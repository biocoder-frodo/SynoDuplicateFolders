using Renci.SshNet;
using System;
namespace SynoDuplicateFolders.Data.SecureShell
{
    public enum TerminalParse
    {
        Quit = 0
    }
    public class TerminalParseResult
    {

        public readonly string Message;
        public readonly Enum NextStep;
        public readonly string StreamData;
        public readonly bool Success;
        public TerminalParseResult(string streamData, bool success, string message)
        {
            Success = success;
            StreamData = streamData;
            NextStep = TerminalParse.Quit;
            Message = message;
        }
        public TerminalParseResult(string streamData, Enum parseStep)
        {
            Success = true;
            StreamData = streamData;
            Message = null;
            NextStep = parseStep;
        }
    }
    public static class Extensions
    {
        public static TerminalParseResult Execute(this ShellStream shellStream, TerminalParseStep step)
        {
            if (step.TimeOut.HasValue)
                shellStream.Expect(step.TimeOut.Value, step.ExpectAction);
            else
                shellStream.Expect(step.ExpectAction);

            if (step.Result!=null)System.Diagnostics.Debug.WriteLine($"<StreamData>{step.Result.StreamData}</StreamData>");
            
            return step.Result is null ? new TerminalParseResult(null,false,"No message was returned.") : step.Result;

        }
    }
}
