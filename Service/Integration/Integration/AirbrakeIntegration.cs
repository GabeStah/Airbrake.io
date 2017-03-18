using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Sharpbrake.Client;

namespace Integration
{
    class AirbrakeIntegration
    {
        public AirbrakeConfig Config { get; set; }
        public AirbrakeNotifier Notifier { get; set; }
        public Dictionary<string, string> Settings { get; set; }

        public AirbrakeIntegration()
        {
            Settings = ConfigurationManager.AppSettings.AllKeys
                .Where(key => key.StartsWith("Airbrake", StringComparison.OrdinalIgnoreCase))
                .ToDictionary(key => key, key => ConfigurationManager.AppSettings[key]);

            Config = AirbrakeConfig.Load(Settings);
            Notifier = new AirbrakeNotifier(Config);
        }
    }
}
