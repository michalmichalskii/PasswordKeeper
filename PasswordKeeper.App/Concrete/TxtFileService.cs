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

namespace PasswordKeeper.App.Concrete
{
    public class TxtFileService
    {
        private const string PASSWORDS_FILE_PATH = "hasła.txt";

        private UserDataManager _passwordManager;
        public TxtFileService(UserDataManager passwordManager)
        {
            _passwordManager = passwordManager;
        }

        public void SaveToTxtFile()
        {
            var passwords = _passwordManager.GetPasswordsList();
            int i = 0;

            using (var sw = new StreamWriter(PASSWORDS_FILE_PATH, true))
            {
                foreach (var password in passwords)
                {
                    var encryptedPassword = EncryptionsMaker.MakeCesarEncryption(password.PasswordString);

                    sw.WriteLine($"{password.Site}|{password.EmailOrLogin}|{encryptedPassword}");

                    i++;
                }
            }
            Console.WriteLine($"Operation completed. {i} rows affected");
        }
    }
}
