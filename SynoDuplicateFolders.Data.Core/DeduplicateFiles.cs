using Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SynoDuplicateFolders.Data.Core
{
    public class DeduplicationRequestStatusEventArgs : EventArgs
    {
        public readonly string StatusMessage;
        public readonly int ProgressValue;
        public readonly int? ProgressMaximum;

        public DeduplicationRequestStatusEventArgs(string statusMessage, int maximumValue)
            : this(statusMessage, maximumValue, 0) { }

        public DeduplicationRequestStatusEventArgs(string statusMessage, int maximumValue, int progressValue)
        {
            StatusMessage = statusMessage;
            ProgressValue = progressValue;
            ProgressMaximum = maximumValue;
        }
        public DeduplicationRequestStatusEventArgs(int progressValue)
        {
            ProgressValue = progressValue;
        }
    }

    public class DeduplicationConfirmationEventArgs : EventArgs
    {
        public readonly string[] Message;
        public DeduplicationConfirmationEventArgs(string[] message)
        {
            Message = message;
        }
    }

    public class DeduplicationInformationEventArgs : EventArgs
    {
        public readonly string Message;

        public DeduplicationInformationEventArgs(string message)
        {
            Message = message;
        }
    }
    public class Deduplication
    {
        public delegate void DeduplicationRequestStatusHandler(object sender, DeduplicationRequestStatusEventArgs e);
        public delegate void DeduplicationInformationHandler(object sender, DeduplicationInformationEventArgs e);
        public delegate void DeduplicationConfirmationHandler(object sender, DeduplicationConfirmationEventArgs e);

        public event DeduplicationRequestStatusHandler OnDeduplicationRequestStatusUpdate;
        public event DeduplicationInformationHandler OnDeduplicationInformationUpdate;
        public event DeduplicationConfirmationHandler OnDeduplicationConfirmation;

        private bool scanned = false;
        private readonly List<FileInfo> toBeRemoved = new List<FileInfo>();
        private Action<string> logMessage = null;
        private Func<string[], bool> acceptFunc = null;

        public Deduplication()
        {
            logMessage = (s) => OnDeduplicationInformationUpdate?.Invoke(this, new DeduplicationInformationEventArgs(s));
            acceptFunc = (msg) => AskTheDialog(msg);
        }
        private Deduplication(Action<string> logMessage, Func<string[], bool> acceptFunc)
        {
            this.logMessage = logMessage;
            this.acceptFunc = acceptFunc;
        }
        public static void DeduplicateFiles(List<DirectoryInfo> directories, bool confirmed = false)
        {
            new Deduplication(s => Console.WriteLine(s),
                            msg =>
                            {
                                foreach (var s in msg) Console.WriteLine(s);
                                var response = Console.ReadLine().ToLower().Trim(); return response == "yes";
                            })
                .DeduplicateFiles(directories, confirmed, false);
        }
        public void DeduplicateFiles(List<DirectoryInfo> directories)
        {
            DeduplicateFiles(directories, false, true);
        }
        private void DeduplicateFiles(List<DirectoryInfo> directories, bool confirmed, bool raiseEvents)
        {
            long totalSize = 0;

            var paths = directories.Where(di => di.Exists).ToDictionary(k => k.FullName);
            var files = new Dictionary<string, Dictionary<string, RelativeFileInfo>>();

            if (paths.Count < 2 || scanned)
            {
                logMessage?.Invoke("Nothing to do.");
                return;
            }

            if (raiseEvents) OnDeduplicationRequestStatusUpdate?.Invoke(this, new DeduplicationRequestStatusEventArgs("Reading folders", paths.Count * 2));

            var keepPath = directories[0];
            var keep = RelativeFileInfo.GetFiles(keepPath);

            int folder = 1;
            if (raiseEvents) OnDeduplicationRequestStatusUpdate?.Invoke(this, new DeduplicationRequestStatusEventArgs(folder));


            paths.Keys.ToList().ForEach(p =>
            {
                if (keepPath.FullName != p)
                {
                    files.Add(p, RelativeFileInfo.GetFiles(paths[p]));
                    if (raiseEvents) OnDeduplicationRequestStatusUpdate?.Invoke(this, new DeduplicationRequestStatusEventArgs(++folder));
                }
            });

            var toBeScanned = new List<KeyValuePair<FileInfo, FileInfo>>();

            foreach (var alternative in files.Values)
            {
                foreach (var file in alternative.Keys)
                {
                    if (keep.ContainsKey(file))
                    {
                        toBeScanned.Add(new KeyValuePair<FileInfo, FileInfo>(keep[file].SourcePath, alternative[file].SourcePath));
                    }
                }
            }
            if (toBeScanned.Any())
            {
                folder = toBeScanned.Count;
                if (raiseEvents)
                {
                    OnDeduplicationRequestStatusUpdate?.Invoke(this, new DeduplicationRequestStatusEventArgs("Scanning files", toBeScanned.Count * 2, folder));
                }

                foreach (var scan in toBeScanned)
                {
                    folder++;
                    if (CompareFiles(logMessage, scan.Key, scan.Value))
                    {
                        totalSize += scan.Key.Length;
                        toBeRemoved.Add(scan.Value);
                    }
                    if (raiseEvents) OnDeduplicationRequestStatusUpdate?.Invoke(this, new DeduplicationRequestStatusEventArgs(folder));
                }
            }
            scanned = true;

            if (toBeRemoved.Count != 0)
            {

                if (!confirmed && acceptFunc != null)
                {
                    confirmed = acceptFunc(new string[]
                    {
                    $"Removing the duplicate(s) would save { totalSize.ToFileSizeString() } of space on your device.",
                    "Would you like to delete the duplicates now? (yes/no)?"
                    });
                }

                if (confirmed)
                {
                    AcceptDeletes();
                    return;
                }
            }
            else
            {
                OnDeduplicationRequestStatusUpdate?.Invoke(this, new DeduplicationRequestStatusEventArgs("Nothing to do.", 100, 0));
            }
            if (!raiseEvents) logMessage?.Invoke("Nothing to do.");

        }
        public void AcceptDeletes()
        {
            var removeFolders = new Dictionary<string, DirectoryInfo>();
            toBeRemoved.ForEach((r) => { if (removeFolders.ContainsKey(r.Directory.FullName) == false) removeFolders.Add(r.Directory.FullName, r.Directory); });

            int file = 1;
            logMessage?.Invoke("Removing files ...");
            OnDeduplicationRequestStatusUpdate?.Invoke(this, new DeduplicationRequestStatusEventArgs("Removing files ...", toBeRemoved.Count));
            foreach (var removeFile in toBeRemoved)
            {
                try
                {
                    removeFile.Delete();
                }
                catch (Exception ex)
                {
                    logMessage?.Invoke($"Removing '{removeFile.FullName}': {ex.Message}");
                }
                OnDeduplicationRequestStatusUpdate?.Invoke(this, new DeduplicationRequestStatusEventArgs(file++));
            }

            int folder = 1;
            OnDeduplicationRequestStatusUpdate?.Invoke(this, new DeduplicationRequestStatusEventArgs("Removing folders ...", removeFolders.Count));
            foreach (var removeFolder in removeFolders.Values.OrderByDescending(f => f.FullName.Length))
            {
                try
                {
                    DeleteEmptyFolders(removeFolder, true);
                }
                catch (Exception ex)
                {
                    logMessage?.Invoke($"Removing '{removeFolder.FullName}': {ex.Message}");
                }
                OnDeduplicationRequestStatusUpdate?.Invoke(this, new DeduplicationRequestStatusEventArgs(folder++));
            }
            OnDeduplicationRequestStatusUpdate?.Invoke(this, new DeduplicationRequestStatusEventArgs(string.Empty, 100, 0));
            logMessage?.Invoke("Done.");
        }
        private bool AskTheDialog(string[] question)
        {
            var e = new DeduplicationConfirmationEventArgs(question);
            OnDeduplicationConfirmation?.Invoke(this, e);
            return false;
        }
        private static bool CompareFiles(Action<string> logMessage, FileInfo file0, FileInfo file1)
        {
            if (file0.Length != file1.Length) return false;
            try
            {
                //logMessage($"{file1.FullName} may be a duplicate of {file0.FullName}");
                using (FileStream s1 = new FileStream(file0.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (FileStream s2 = new FileStream(file1.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (BinaryReader b1 = new BinaryReader(s1))
                using (BinaryReader b2 = new BinaryReader(s2))
                {
                    byte[] data1 = new byte[64 * 1024 * 1024];
                    byte[] data2 = new byte[64 * 1024 * 1024];

                    while (true)
                    {


                        var t1 = Task.Run(() => b1.ReadBytes(data1.Length));
                        var t2 = Task.Run(() => b2.ReadBytes(data2.Length));
                        Task.WaitAll(t1, t2);
                        data1 = t1.Result;
                        data2 = t2.Result;

                        if (data1.Length != data2.Length)
                            return false;
                        if (data1.Length == 0)
                        {
                            logMessage($"{file1.FullName} is a duplicate of {file0.FullName}");
                            return true;
                        }
                        if (!data1.SequenceEqual(data2))
                            return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static void DeleteEmptyFolders(string path, bool recursive = false)
        {
            bool changed = true;
            if (recursive)
            {
                while (changed)
                {
                    changed = false;
                    DeleteEmptyFolders(ref changed, path);
                }
                return;
            }

            DeleteEmptyFolders(ref changed, path);
        }
        public static void DeleteEmptyFolders(DirectoryInfo folder, bool recursive = false)
        {
            if (folder is null) throw new ArgumentNullException(nameof(folder));
            if (folder.Exists)
            {
                DeleteEmptyFolders(folder.FullName, recursive);
            }
            else throw new DirectoryNotFoundException();
        }
        private static void DeleteEmptyFolders(ref bool changed, string path)
        {
            try
            {

                var entries = Directory.EnumerateFileSystemEntries(path);
                foreach (var d in Directory.EnumerateDirectories(path))
                {
                    DeleteEmptyFolders(ref changed, d);
                }
                if (!entries.Any())
                {


                    Directory.Delete(path);

                    changed = true;

                }
            }
            catch (UnauthorizedAccessException) { }
            catch (DirectoryNotFoundException) { }

        }

    }
}
