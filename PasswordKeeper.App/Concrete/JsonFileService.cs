using PasswordKeeper.App.Common;
using PasswordKeeper.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordKeeper.Cypher;
using PasswordKeeper.App.Managers;
using PasswordKeeper.Domain.Common;
using Newtonsoft.Json;
using PasswordKeeper.App.Abstarct;
using PasswordKeeper.App.Settings;
using System.Runtime.InteropServices.ComTypes;
using ConsoleTables;

namespace PasswordKeeper.App.Concrete
{
    public class JsonFileService
    {
        private readonly IService<User> _userDataService;
        public JsonFileService(IService<User> userDataService)
        {
            _userDataService = userDataService;
        }

        public string SerializeObjectsToJsonFileFormat()
        {
            var users = _userDataService.GetAllItems();
            foreach (var user in users)
            {
                user.PasswordString = EncryptionsMaker.EncryptPlainTextToCipherText(user.PasswordString);
            }
            string jsonOutput = JsonConvert.SerializeObject(users);
            return jsonOutput;
        }

        public List<User> DeserializeObjectsFromJsonFormat(string jsonString)
        {
            var users = JsonConvert.DeserializeObject<List<User>>(jsonString);
            return users;
        }

        private string GetFilePath()
        {
            var jsonString = File.ReadAllText("appsettings.json");
            var appConfig = JsonConvert.DeserializeObject<AppConfig>(jsonString);
            string filePath = appConfig.PassordsFilePath;
            return filePath;
        }

        public void UpdateJsonFile()
        {
            string updatedJson = SerializeObjectsToJsonFileFormat();
            using var sw = new StreamWriter(GetFilePath(), false);
            sw.Write(updatedJson);
        }

        public int GetLastIdFromJson()
        {
            var users = GetAllUsersFromJson();
            int lastId = 0;
            if (users.Count > 0)
                lastId = users.LastOrDefault().Id;
            return lastId;
        }

        public List<User> GetAllUsersFromJson()
        {
            string filePath = GetFilePath();
            using var sr = new StreamReader(filePath);
            string getText = "";
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                getText += line;
            }

            var users = DeserializeObjectsFromJsonFormat(getText);
            foreach (var userData in users)
            {
                userData.PasswordString = DecryptionMaker.DecryptCipherTextToPlainText(userData.PasswordString);
            }
            if (users != null)
            {
                return users;
            }
            else
            {
                return _userDataService.Items;
            }
        }
    }
}
