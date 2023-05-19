using Renci.SshNet;
using System;
using System.Text.RegularExpressions;

namespace SynoDuplicateFolders.Data.SecureShell
{
    public delegate TerminalParseResult ExecuteStep(string message);
    public class TerminalParseStep
    {

        private ExpectAction[] expectAction;
        internal ExpectAction[] ExpectAction => expectAction;
        public TerminalParseResult Result { get; private set; }

        public TimeSpan? TimeOut { get; set; }
        private void ExecuteStepAction(string text, ExecuteStep executeStep)
        {
            Result = executeStep(text);
        }
        public TerminalParseStep(string expect, ExecuteStep executeStep)
        {
            Initialize(expect, executeStep);
        }
        public TerminalParseStep(Regex expect, ExecuteStep executeStep)
        {
            Initialize(expect, executeStep);
        }
        public TerminalParseStep(string[] expect, ExecuteStep[] executeStep)
        {
            Initialize(expect, executeStep);
        }
        public TerminalParseStep(Regex[] expect, ExecuteStep[] executeStep)
        {
            Initialize(expect, executeStep);
        }
        private void Initialize<T>(T expect, ExecuteStep executeStep) where T : class
        {
            Initialize(new T[] { expect }, new ExecuteStep[] { executeStep });
        }
        private void Initialize<T>(T[] expect, ExecuteStep[] executeStep) where T : class
        {
            if (expect.Length != executeStep.Length) throw new ArgumentOutOfRangeException("expect", "The input arrays should have identical length.");
            expectAction = new ExpectAction[executeStep.Length];
            int idx;
            for (idx = 0; idx < expect.Length; idx++)
            {
                var stepInstance = executeStep[idx];
                if (typeof(T) == typeof(Regex))
                    expectAction[idx] = new ExpectAction(expect[idx] as Regex, (s) => { ExecuteStepAction(s, stepInstance); });
                if (typeof(T) == typeof(string))
                    expectAction[idx] = new ExpectAction(expect[idx] as string, (s) => { ExecuteStepAction(s, stepInstance); });

            }
        }
    }
}
