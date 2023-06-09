using Newtonsoft.Json;
using PasswordKeeper.App.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordKeeper.Helpers
{
    public static class FilesHelper
    {
        public static T LoadData<T>(string jsonFile)
        {
            if (!File.Exists(jsonFile))
                using (File.Create(jsonFile)) ;

            var jsonString = File.ReadAllText(jsonFile);
            var appConfig = JsonConvert.DeserializeObject<T>(jsonString);
            return appConfig;
        }
        public static void SaveData<T>(List<T> listOfData, string jsonFile)
        {
            if (!File.Exists(jsonFile))
                using (File.Create(jsonFile)) ;

            string jsonOutput = JsonConvert.SerializeObject(listOfData);

            string updatedJson = jsonOutput;
            using var sw = new StreamWriter(jsonFile, false);
            sw.Write(updatedJson);
        }
    }
}
