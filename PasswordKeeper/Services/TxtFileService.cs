using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordKeeper.Enums;

namespace PasswordKeeper.Services
{
    public class TxtFileService
    {
        //this is individual class, because maybe in future will be more things connected with operation on txt files

        private const string PASSWORDS_FILE_PATH = "hasła.txt";

        private PasswordService _passwordService;
        public TxtFileService(PasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        public void SaveToTxtFile()
        {
            Dictionary<string, List<string>> passwords = _passwordService.GetPasswordsWithSites();
            int i = 0;

            using (var sw = new StreamWriter(PASSWORDS_FILE_PATH, true))
            {
                foreach (var password in passwords)
                {
                    var emailAndPassword = passwords.Values.ToArray();
                    var encryptedEmail = _passwordService.MakeEncryptionOrDescryption(emailAndPassword[i][0],Cipher.Encryption);
                    var encryptedPassword = _passwordService.MakeEncryptionOrDescryption(emailAndPassword[i][1],Cipher.Encryption);

                    sw.WriteLine($"{password.Key}|{encryptedEmail}|{encryptedPassword}");

                    i++;

                    //if program needs to decypher code - below is example how to do this (maybe someday it will come in handy???)
                    //var descryptedEmail = _passwordService.MakeEncryptionOrDescryption(encryptedEmail, Cipher.Descryption);
                    //var descryptedPassword = _passwordService.MakeEncryptionOrDescryption(encryptedPassword, Cipher.Descryption);
                    //sw.WriteLine($"{password.Key}|{descryptedEmail}|{descryptedPassword}");
                }
            }
            Console.WriteLine($"Operation completed. {i} rows affected");
        }
    }
}
