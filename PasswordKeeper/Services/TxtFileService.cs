using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    sw.WriteLine($"{password.Key}|{password.Value}");
                    i++;
                }
            }
            Console.WriteLine($"Operation completed. {i} rows affected");
        }
    }
}
