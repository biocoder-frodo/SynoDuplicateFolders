using System;

namespace SynoDuplicateFolders.Extensions
{
    public static partial class Extensions
    {
        private const int default_precision = 1;
        private static readonly FileSizeFormatProvider _talk_bytes = new FileSizeFormatProvider(default_precision);

        public static string ToFileSizeString(this long l, int precision = default_precision)
        {
            return String.Format(_talk_bytes, "{0:fs" + precision.ToString() + "}", l);
        }

        public static string ToFileSizeString(this long l, FileSizeFormatSize range, bool suffix = true, int precision = default_precision)
        {
            return String.Format(_talk_bytes, "{0:fs" + precision.ToString() + "," + range.ToString() + (suffix ? "":"!") + "}", l);
        }
    }
    public enum FileSizeFormatSize
    {
        B,
        kB,
        MB,
        GB,
        TB,
        PB
    }
    public class FileSizeFormatProvider : IFormatProvider, ICustomFormatter
    {
        private readonly int _default_precision;
        public FileSizeFormatProvider()
            : this(1)
        { }
        public FileSizeFormatProvider(int default_precision)
        {
            _default_precision = default_precision;
        }
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter)) return this;
            return null;
        }

        private const string fileSizeFormat = "fs";
        private const Decimal OneKiloByte = 1024M;
        private const Decimal OneMegaByte = OneKiloByte * 1024M;
        private const Decimal OneGigaByte = OneMegaByte * 1024M;
        private const Decimal OneTeraByte = OneGigaByte * 1024M;
        private const Decimal OnePetaByte = OneTeraByte * 1024M;

        private Decimal[] sizes = new Decimal[6]
        {
            1M, OneKiloByte, OneMegaByte, OneGigaByte, OneTeraByte, OnePetaByte
        };

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (format == null || !format.StartsWith(fileSizeFormat))
            {
                return defaultFormat(format, arg, formatProvider);
            }

            if (arg is string)
            {
                return defaultFormat(format, arg, formatProvider);
            }
            string precision;
            FileSizeFormatSize range = FileSizeFormatSize.B;
            bool hasRange = false;
            bool noSuffix = false;
            if (format.Contains(","))
            {
                var args = format.Split(',');
                precision = args[0].Substring(2);
                string r = args[1];
                if (r.EndsWith("!"))
                {
                    noSuffix = true;
                    r = r.Substring(0, r.Length - 1);
                }
                hasRange = true;
                range = (FileSizeFormatSize)Enum.Parse(typeof(FileSizeFormatSize),r);
                
            }
            else
            {
                range = FileSizeFormatSize.PB;
                precision = format.Substring(2);
            }

            if (String.IsNullOrEmpty(precision)) precision = _default_precision.ToString();

            Decimal size;
            try
            {
                size = Convert.ToDecimal(arg);
            }
            catch (InvalidCastException)
            {
                return defaultFormat(format, arg, formatProvider);
            }

            string suffix = null;


            if (hasRange)
            {
                suffix = range.ToString();
                size /= sizes[(int)range];
            }
            else
            {
                while (suffix == null)
                {
                    if (size > sizes[(int)range])
                    {
                        size /= sizes[(int)range];
                        suffix = range.ToString();
                    }
                    else
                    {
                        if (--range == 0) suffix = range.ToString();
                    }
                }
            }
            if (range == FileSizeFormatSize.B || size == 0) precision = "0";
            
            return noSuffix ? String.Format("{0:N" + precision + "}", size) : String.Format("{0:N" + precision + "} {1}", size, suffix);

        }

        private static string defaultFormat(string format, object arg, IFormatProvider formatProvider)
        {
            IFormattable formattableArg = arg as IFormattable;
            if (formattableArg != null)
            {
                return formattableArg.ToString(format, formatProvider);
            }
            return arg.ToString();
        }

    }
}