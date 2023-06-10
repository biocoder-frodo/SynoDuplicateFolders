using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DiskStationManager.SecureShell
{
    interface IKeyboardInteractiveKeyPress
    {
        event KeyPressEventHandler KeyPress;
    }
    class SudoSession<F> : SudoSession where F : Form, IKeyboardInteractiveKeyPress, new()
    {
        public SudoSession(ConnectionInfo connectionInfo) : base(connectionInfo, ()=>new F()) { }
    }
    class SudoSession
    {
        private static readonly Regex regexPrompt = new Regex(@"[$#>]", RegexOptions.Compiled | RegexOptions.Singleline);
        private static readonly Regex regexPromptForPassword = new Regex(@"([$#>]|(Password:)|(sudo:))", RegexOptions.Compiled | RegexOptions.Singleline);
        private static string sudo(string command)
        {
            return command.StartsWith("cd ")? command: $"sudo {command}";
        }
        private readonly ConsoleCommandMode _mode;
        private readonly ConnectionInfo _connectionInfo;
        private readonly Func<string> _password;
        private readonly Func<Form> _interactivePassword;

        public SudoSession(ISecureShellSession secureShell)
            :this(secureShell.ConnectionInfo,secureShell.GetPassword)
        { }
        public SudoSession(ConnectionInfo connectionInfo, Func<string> passwordGetter)
            : this(connectionInfo, ConsoleCommandMode.InteractiveSudo)
        {
            _password = passwordGetter;
        }
        public SudoSession(ConnectionInfo connectionInfo, ConsoleCommandMode mode)
        {
            _mode = mode;
            _connectionInfo = connectionInfo;
        }
        internal SudoSession(ConnectionInfo connectionInfo, Func<Form> passwordForm)
        {
            _mode = ConsoleCommandMode.InteractiveSudo;
            _interactivePassword = passwordForm;
        }

        public void Run(string command)
        {
            Run(new string[] { command });
        }
        public void Run(string[] commands)
        {
            using (SshClient scr = new SshClient(_connectionInfo))
            {
                scr.Connect();
                RunCommands(scr, commands, _mode, _password);
                scr.Disconnect();
            }
        }

        internal static string RunCommand(SshClient session, string command, ConsoleCommandMode mode, Func<string> passwordGetter = null, Func<Form> func =null)
        {

            switch (mode)
            {
                case ConsoleCommandMode.Sudo:
                    return session.RunCommand(sudo(command)).Result;
                case ConsoleCommandMode.InteractiveSudo:
                    ExperimentalSudo(session, command, passwordGetter,func);
                    return string.Empty;
                default:
                    return session.RunCommand(command).Result;
            }
        }
        internal static string RunCommands(SshClient session, string[] commands, ConsoleCommandMode mode, Func<string> passwordGetter = null, Func<Form> func = null)
        {
            if (mode == ConsoleCommandMode.InteractiveSudo)
            {
                ExperimentalSudo(session, commands, passwordGetter,func);
                return string.Empty;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                for (int idx = 0; idx < commands.Length - 1; idx++)
                {
                    sb.Append(RunCommand(session, commands[idx], mode, passwordGetter));
                    sb.Append(Environment.NewLine);
                }
                return sb.ToString();
            }
        }

        private static void ExperimentalSudo(SshClient session, string command, Func<string> passwordGetter, Func<Form> func)
        {
            ExperimentalSudo(session, new string[] { command }, passwordGetter, func);
        }
        private static void ExperimentalSudo(SshClient session, string[] command, Func<string> passwordGetter, Func<Form> func)
        {
            try
            {
                var termkvp = new Dictionary<TerminalModes, uint> { { TerminalModes.ECHO, 53 } };

                using (ShellStream shellStream = session.CreateShellStream("xterm", 255, 24, 800, 600, 1024, termkvp))
                {
                    ExperimentalSudo(shellStream, command, passwordGetter,func);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                throw;
            }

        }
        internal enum SudoStates
        {
            promptAppears = 1,
            passwordChallenge,

        }
        internal class TerminalParseSteps<E> : Dictionary<E, TerminalParseStep> where E : Enum { }
        internal class TerminalParseSteps : TerminalParseSteps<SudoStates>
        { }

        private static void DebugInfo(string message)
        { System.Diagnostics.Debug.WriteLine($"{DateTime.UtcNow.ToString("u")} <Message>{message.Replace("\r", "\\r").Replace("\n", "\\n")}</Message>"); }
        private static void ExperimentalSudo(ShellStream shellStream, string[] command, Func<string> passwordGetter, Func<Form> func)
        {
            //////sudo ls .
            //////Password:
            //////Sorry, try again.
            //////Password:
            //////Sorry, try again.
            //////Password:
            //////sudo: 3 incorrect password attempts


            int idx = 0;
            bool elevated = false;
            var sudoStates = new TerminalParseSteps();

            sudoStates.Add(SudoStates.passwordChallenge, new TerminalParseStep(regexPromptForPassword,
                s =>
                {
                    DebugInfo(s);

                    if (s.Contains("Password:"))
                    {
                        if (passwordGetter != null)
                        {
                            if (string.IsNullOrWhiteSpace(passwordGetter())) MessageBox.Show("You did not supply your password");
                            elevated = false;
                            System.Threading.Thread.Sleep(100);
                            shellStream.WriteLine(passwordGetter());
                            return new TerminalParseResult(s, SudoStates.passwordChallenge);
                        }
                        if (func != null)
                        { 
                        }
                    }
                    if (s.Contains("sudo:"))
                    {
                        return new TerminalParseResult(s, false, "The privilege elevation failed.");
                    }

                    elevated = true;
                    System.Diagnostics.Debug.WriteLine("Waiting for command to complete");
                    idx++; //prepare for the next command
                    if (idx < command.Length)
                    {
                        DebugInfo(sudo(command[idx]));
                        shellStream.WriteLine(sudo(command[idx]));
                        return new TerminalParseResult(s, SudoStates.passwordChallenge);
                    }

                    return new TerminalParseResult(s, TerminalParse.Quit);
                })
            {
                TimeOut = new TimeSpan(0, 1, 0)
            });
            sudoStates.Add(SudoStates.promptAppears, new TerminalParseStep(regexPrompt,
                s =>
                {
                    DebugInfo(s);
                    if (idx < command.Length)
                    {
                        DebugInfo(sudo(command[idx]));
                        shellStream.WriteLine(sudo(command[idx]));
                        return new TerminalParseResult(s, SudoStates.passwordChallenge);
                    }

                    return new TerminalParseResult(s, TerminalParse.Quit);
                }));


            var state = SudoStates.promptAppears;

            while (state != 0)
            {
                System.Diagnostics.Debug.WriteLine(state);
                var expect = shellStream.Execute(sudoStates[state]);

                if (expect.Success == false)
                {
                    System.Diagnostics.Debug.WriteLine($"unexpected text: {expect.Message}"); break;
                }

                state = (SudoStates)expect.NextStep;
            }
        }

    }

}