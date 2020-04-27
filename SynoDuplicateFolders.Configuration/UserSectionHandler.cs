using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace SynoDuplicateFolders.Configuration
{
    public static class UserSectionHandler
    {
        public static T GetSection<T>() where T: ConfigurationSection, new()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
            if (config.Sections.OfType<T>().Count() == 0)
            {
                var cs = new T();
                cs.SectionInformation.AllowExeDefinition = ConfigurationAllowExeDefinition.MachineToLocalUser;

                config.Sections.Add(typeof(T).Name, cs);

                //config.Save();
                //cs.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                config.Save();
                ConfigurationManager.RefreshSection(cs.SectionInformation.Name);
            };

            return config.Sections.OfType<T>().First();
        }
    }
}
