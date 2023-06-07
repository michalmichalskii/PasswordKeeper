using PasswordKeeper.Cypher;
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

        public string AdminPassword { get; set; }

        public void SetConfig()
        {
            if (File.Exists("appsettings.json"))
                return;

            var appSettings = new AppConfig
            {
                PassordsFilePath = "passwords.json",
                MaxAttempts = 3,
                SecurityKey = "TMPSecK3yAmg1g18ait",
            };
            string jsonString = JsonSerializer.Serialize(appSettings);
            File.WriteAllText("appsettings.json", jsonString);

            appSettings.AdminPassword = EncryptionsMaker.EncryptPlainTextToCipherText("admin1");
            jsonString = JsonSerializer.Serialize(appSettings);
            File.WriteAllText("appsettings.json", jsonString);
        }
    }
}

