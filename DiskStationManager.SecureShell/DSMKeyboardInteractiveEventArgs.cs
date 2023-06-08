using System;
using Renci.SshNet.Common;

namespace DiskStationManager.SecureShell
{
    public class DSMKeyboardInteractiveEventArgs: EventArgs
    {
        public readonly string Banner;
        public readonly string Instruction;
        public readonly string Language;
        public readonly string Username;
        public readonly string Request;
        public readonly int Id;
        public readonly bool IsEchoed;

        internal DSMKeyboardInteractiveEventArgs(AuthenticationBannerEventArgs abe, AuthenticationPromptEventArgs ape, AuthenticationPrompt p)
        {
            if (abe != null) { Banner = abe.BannerMessage; } else { Banner = string.Empty; }
            Instruction = ape.Instruction;
            Language = ape.Language;
            Username = ape.Username;
            Request = p.Request;
            Id = p.Id;
            IsEchoed = p.IsEchoed;
        }
    }
}
