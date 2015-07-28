using System;
using CodeCharm.Utility.Properties;

namespace CodeCharm.Utility
{
    public static class Build
    {
        private static BuildConfiguration? c_configuration = null;
        public static BuildConfiguration Configuration
        {
            get
            {
                if (!c_configuration.HasValue)
                {
                    string configuration = Settings.Default.Configuration;
                    BuildConfiguration buildConfiguration;
                    c_configuration = Enum.TryParse(configuration, true, out buildConfiguration) ? buildConfiguration : BuildConfiguration.NotSet;
                }
                return c_configuration.Value;
            }
        }

        private static bool? c_isProdOrStag = null;
        public static bool IsProdOrStagConfiguration
        {
            get
            {
                if (!c_isProdOrStag.HasValue)
                {
                    c_isProdOrStag = (Configuration == BuildConfiguration.Prod
                                      || Configuration == BuildConfiguration.ProdCore
                                      || Configuration == BuildConfiguration.MpStag
                                      || Configuration == BuildConfiguration.Stag
                                      || Configuration == BuildConfiguration.StagCore
                                     );
                }
                return c_isProdOrStag.Value;
            }
        }

        private static bool? c_isProd = null;
        public static bool IsProdConfiguration
        {
            get
            {
                if (!c_isProd.HasValue)
                {
                    c_isProd = (Configuration == BuildConfiguration.Prod
                                      || Configuration == BuildConfiguration.ProdCore
                                      || Configuration == BuildConfiguration.MpStag
                                     );
                }
                return c_isProd.Value;
            }
        }

    }
}
