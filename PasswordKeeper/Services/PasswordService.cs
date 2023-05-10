using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PasswordKeeper.Services
{
    public class PasswordService
    {
        private WebService _service;

        private List<string> _listOfUserDataForSite;
        private Dictionary<string, List<string>> _passwords;


        public PasswordService(WebService service)
        {
            _passwords = new Dictionary<string, List<string>>();
            _service = service;
        }

        private bool CheckIsInputFilled(string checkedInput)
        {
            if (string.IsNullOrEmpty(checkedInput))
            {
                throw new Exception("You cannot leave this space null");
            }

            return true;
        }

        public Dictionary<string, List<string>> GetPasswordsWithSites()
        {
            return _passwords;
        }

        public void AddNewPassword()
        {
            Console.WriteLine("Please enter URL of a site for your password: ");
            var readSite = Console.ReadLine();
            Console.WriteLine("Please enter your Email for this site: ");
            var readEmail = Console.ReadLine();
            Console.WriteLine("Now please enter your password: ");
            var readPassword = Console.ReadLine();

            var password = new Password();

            CheckIsInputFilled(readEmail);
            CheckIsInputFilled(readSite);
            CheckIsInputFilled(readPassword);

            _service.CheckIsSiteAvailable(readSite);

            password.SiteOfPassword = readSite;
            password.EmailOrLogin = readEmail;
            password.PasswordString = readPassword;

            _listOfUserDataForSite = new List<string>
            {
                readEmail,
                readPassword
            };

            _passwords.Add(password.SiteOfPassword, _listOfUserDataForSite);
            Console.WriteLine("Password added correctly");
        }

        public void RemovePassword()
        {
            Console.WriteLine("Enter a site of password that you want to delete: ");
            var readSite = Console.ReadLine();

            CheckIsInputFilled(readSite);

            if (_passwords.ContainsKey(readSite) == false)
            {
                throw new Exception("You have not saved password for this site");
            }

            Console.WriteLine("Enter a email for site that you want to delete: ");
            var readEmail = Console.ReadLine();
            Console.WriteLine("Enter a password that you want to delete: ");
            var readPassword = Console.ReadLine();

            CheckIsInputFilled(readEmail);
            CheckIsInputFilled(readPassword);

            _listOfUserDataForSite = new List<string>
            {
                readEmail,
                readPassword
            };

            var removed = _passwords.Remove(readSite, out _listOfUserDataForSite); //can be empty list?

            if (removed == false)
            {
                Console.WriteLine("Password or email is incorrect");
            }

        }

        public void ChangePassword()
        {
            Console.WriteLine("Enter a site of password that you want to change: ");
            var readSite = Console.ReadLine();

            CheckIsInputFilled(readSite);

            if (_passwords.ContainsKey(readSite) == false)
            {
                throw new Exception("You have not saved password for this site");
            }

            Console.WriteLine("Enter a email for site where you want to change a password: ");
            var readEmail = Console.ReadLine();

            CheckIsInputFilled(readEmail);

            Console.WriteLine("Enter a password that you want to delete: ");
            var readPassword = Console.ReadLine();

            CheckIsInputFilled(readPassword);

            _listOfUserDataForSite = new List<string>
            {
                readEmail,
                readPassword
            };

            for (int i = 0; i < _passwords.Count; i++)
            {
                if (_passwords.TryGetValue(readSite, out _listOfUserDataForSite))
                {
                    var removed = _passwords.Remove(readSite, out _listOfUserDataForSite);

                    if (removed == false)
                    {
                        throw new Exception("Incorrect password");
                    }
                }
            }

            _listOfUserDataForSite.Clear();

            Console.WriteLine("Enter a new password: ");
            var newReadPassword = Console.ReadLine();

            CheckIsInputFilled(newReadPassword);

            var newPassword = new Password
            {
                SiteOfPassword = readSite,
                EmailOrLogin = readEmail,
                PasswordString = newReadPassword
            };

            _listOfUserDataForSite.Add(readEmail);
            _listOfUserDataForSite.Add(newReadPassword);

            _passwords.Add(readSite, _listOfUserDataForSite);
        }

        public void GetAllPasswordsWithSites()
        {
            int i = 1;
            Console.WriteLine("ID|SITE|EMAIL|PASSWORD");
            foreach (var password in _passwords)
            {
                string[] emailAndPassword = password.Value.ToArray();

                Console.WriteLine($"{i}|{password.Key}|{emailAndPassword[0]}|{emailAndPassword[1]}");
                i++;


            }
        }

        public void FindPasswordOnWrittenSite()
        {
            Console.WriteLine("Enter a site name: ");
            var readSite = Console.ReadLine();

            CheckIsInputFilled(readSite);

            bool isPageFound = false;

            foreach (var password in _passwords)
            {
                if (_passwords.ContainsKey(readSite))
                {
                    var passwordToShow = password.Value.ToArray();
                    Console.WriteLine($"A password for this site is: {passwordToShow[1]}");
                    isPageFound = true;
                    break;
                }
            }

            if (!isPageFound)
            {
                Console.WriteLine($"You do not have any password saved to site: {readSite}");
            }
        }

        public void GenerateRandomPassword()
        {
            char[] alphabet = new[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            char[] numbers = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            char[] specialChars = new[] { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '=', '+', '[', '{', ']', '}', ';', ':', '\'', '\"', ',', '.', '<', '>', '/', '?', '\\' };
            bool hasUpperLetter = false, hasLowerLetter = false, hasSpecial = false, hasNumber = false;

            string randomPassword = "";

            while (!hasLowerLetter && !hasUpperLetter && !hasNumber && !hasSpecial)
            {
                randomPassword = "";

                Random random = new Random();
                var randomLengthOfPassword = random.Next(16, 20);

                for (int i = 0; i < randomLengthOfPassword; i++)
                {
                    var randomNumbers = random.Next(numbers.Length);
                    var randomLetter = random.Next(alphabet.Length);
                    var randomSpecial = random.Next(specialChars.Length);
                    var randomUpperOrNot = random.Next(2);

                    var randomNumberOrLetterOrSpecial = random.Next(1, 4);

                    switch (randomNumberOrLetterOrSpecial)
                    {
                        case 1:
                            if (randomUpperOrNot == 1)
                            {
                                randomPassword += char.ToUpper(alphabet[randomLetter]);
                                hasUpperLetter = true;
                            }
                            else
                            {
                                randomPassword += alphabet[randomLetter];
                                hasLowerLetter = false;
                            }
                            break;
                        case 2:
                            randomPassword += numbers[randomNumbers];
                            hasNumber = true;
                            break;
                        case 3:
                            randomPassword += specialChars[randomSpecial];
                            hasSpecial = true;
                            break;
                    }
                }
            }
            Console.WriteLine(randomPassword);
        }
    }
}

