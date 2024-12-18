using SynoDuplicateFolders.Data.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace DiskStationManager.SecureShell
{
    public sealed partial class DSMHost : ConfigurationElement, IElementProvider, IHostSpecificSettings
    {
        [ConfigurationProperty("keepdsmfiles", IsRequired = false, DefaultValue = false)]
        public bool Custom
        {
            get
            {
                return (bool)this["keepdsmfiles"];
            }
            set
            {
                this["keepdsmfiles"] = value;
            }
        }
        [ConfigurationProperty("keepallfiles", IsRequired = false, DefaultValue = true)]
        public bool KeepAll
        {
            get
            {
                return (bool)this["keepallfiles"];
            }
            set
            {
                this["keepallfiles"] = value;
            }
        }
        [ConfigurationProperty("keepdsmcount", IsRequired = false, DefaultValue = 1)]
        public int KeepCount
        {
            get
            {
                return (int)this["keepdsmcount"];
            }
            set
            {
                this["keepdsmcount"] = value;
            }
        }

        public static string SynoReportHomeDefault(string userName)
        {
            return $"/volume1/homes/{(string.IsNullOrEmpty(userName) ? DefaultUserName : userName)}/";
        }

        [ConfigurationProperty("home", IsRequired = false)]
        public string SynoReportHome
        {
            get
            {
                return this["home"] as string;
            }
            set
            {
                this["home"] = value;
            }
        }

        //public IReadOnlyList<string> Paths
        //{
        //    get
        //    {
        //        return FilterDuplicates.Split('\t').ToList().Where(s => string.IsNullOrWhiteSpace(s) == false).ToList();
        //    }

        //}
        [ConfigurationProperty("dsmdupesfilter", IsRequired = false, DefaultValue = "")]
        public string FilterDuplicates
        {
            get
            {
                System.Diagnostics.Debug.WriteLine($"GET dsmdupesfilter={this["dsmdupesfilter"] as string}");
                return this["dsmdupesfilter"] as string;
            }
            set
            {
                System.Diagnostics.Debug.WriteLine($"SET dsmdupesfilter={value}");
                this["dsmdupesfilter"] = value;
            }
        }
    }
}
