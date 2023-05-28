using SynoDuplicateFolders.Configuration;
using SynoDuplicateFolders.Data.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;

namespace SynoDuplicateFolders.Controls
{
    public class DuplicateCandidatesExclusion<T> : IDuplicateExclusionSource where T : ConfigurationElement, IElementProvider, IHostSpecificSettings
    {
        private readonly List<string> exclusions = new List<string>();
        private readonly Func<T, string> getter;
        private readonly Action<T, string> setter;
        private readonly T instance;
        private string cached;

        public DuplicateCandidatesExclusion(T instance, Func<T, string> getter, Action<T, string> setter)
        {
            this.getter = getter;
            this.setter = setter;
            this.instance = instance;
            cached = getter(instance);
            foreach (var path in cached.Split('\t'))
            {
                if (string.IsNullOrWhiteSpace(path) == false)
                    exclusions.Add(path);
            }
        }
        public IReadOnlyList<string> Paths => exclusions;

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddExclusion(string path)
        {
            if (string.IsNullOrWhiteSpace(path) == false && exclusions.Contains(path) == false)
            {
                exclusions.Add(path);
                UpdateBacking();
            }
        }

        public void RemoveExclusion(string path)
        {
            if (string.IsNullOrWhiteSpace(path) == false && exclusions.Contains(path))
            {
                exclusions.Remove(path);
                UpdateBacking();
            }
        }
        private void UpdateBacking()
        {
            string value = exclusions.Count == 0 ? string.Empty : exclusions.Count == 1 ? exclusions[0] : exclusions.Aggregate((s, a) => $"{s}\t{a}");
            if (cached != value)
            {
                setter(instance, value);
                cached = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Paths"));
            }
        }

        public void RemoveAllExclusions()
        {
            exclusions.Clear();
            UpdateBacking();
        }

        public void AttachDetach()
        {
            UpdateBacking();
        }
    }
}