using PasswordKeeper.App.Common;
using PasswordKeeper.App.Concrete;
using PasswordKeeper.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordKeeper.Cypher;

namespace PasswordKeeper.App.Managers
{
    public class TxtFileManager
    {
        private const string PASSWORDS_FILE_PATH = "hasła.txt";

        private PasswordManager _passwordManager;
        public TxtFileManager(PasswordManager passwordManager)
        {
            _passwordManager = passwordManager;
        }

        public void SaveToTxtFile()
        {
            List<UserDataModel> passwords = _passwordManager.GetPasswordsList();
            int i = 0;

            using (var sw = new StreamWriter(PASSWORDS_FILE_PATH, true))
            {
                foreach (var password in passwords)
                {
                    //var encryptedEmail = EncryptionsMaker.MakeCesarEncryption(password.Value.EmailOrLogin);
                    var encryptedPassword = EncryptionsMaker.MakeCesarEncryption(password.PasswordString);

                    sw.WriteLine($"{password.Site}|{password.EmailOrLogin}|{encryptedPassword}");

                    i++;
                }
            }
            Console.WriteLine($"Operation completed. {i} rows affected");
        }
    }
}
