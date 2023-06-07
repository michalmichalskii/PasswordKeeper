using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PasswordKeeper.App.Settings
{
    public class AppConfig
    {
        public string PassordsFilePath { get; set; }

        public int MaxAttempts { get; set; }

        public string SecurityKey { get; set; }

        public void SetConfig()
        {
            var appSettings = new AppConfig
            {
                PassordsFilePath = "passwords.json",
                MaxAttempts = 3,
                SecurityKey = "TMPSecK3yAmg1g18ait"
            };

            string jsonString = JsonSerializer.Serialize(appSettings);

            File.WriteAllText("appsettings.json", jsonString);
        }
    }
}
