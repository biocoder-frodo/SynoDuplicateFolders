using System;
using SynoDuplicateFolders.Data.Core;

namespace SynoDuplicateFolders.Data
{
    public class SynoCSVReportPair<T1, T2> : ISynoCSVReportPair
        where T1 : class, ISynoCSVReport, new()
        where T2 : class, ISynoCSVReport, new()
    {

        private T1 _first = null;
        private T2 _second = null;

        public SynoCSVReportPair(ISynoCSVReport first, ISynoCSVReport second)
        {
            Initialize(first, second);
        }
        public SynoCSVReportPair(T1 first, T2 second)
        {
            Initialize(first, second);
        }
        public T1 First
        {
            get
            {
                return _first;
            }
        }

        public T2 Second
        {
            get
            {
                return _second;
            }
        }

        ISynoCSVReport ISynoCSVReportPair.First
        {
            get
            {
                return First;
            }
        }

        ISynoCSVReport ISynoCSVReportPair.Second
        {
            get
            {
                return Second;
            }
        }

        public void Initialize(ISynoCSVReport first, ISynoCSVReport second)
        {
            if (first != null && second != null)
            {
                if (((first as T1) != null && (second as T2) != null) ||
                    ((second as T1) != null && (first as T2) != null))
                {
                    if ((first as T1) != null)
                    {
                        _first = first as T1;
                        _second = second as T2;
                    }
                    else
                    {
                        _first = second as T1;
                        _second = first as T2;
                    }
                }
                else
                {
                    string paramName = ((first as T1) == null && (first as T2) == null) == true ? "first" : "second";
                    throw new ArgumentException($"Type mismatch while initializing object, argument should be of type {new T1().GetType().Name} or type {new T2().GetType().Name}.", paramName);
                }
            }
            else
            {
                throw new ArgumentNullException(first == null ? "first" : "second");
            }
        }
    }
}
