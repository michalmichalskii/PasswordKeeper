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

        public Dictionary<string, string> passwords;

        public PasswordService(WebService service)
        {
            passwords = new Dictionary<string, string>();
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

        public Dictionary<string, string> GetPasswordsWithSites()
        {
            return passwords;
        }

        public void AddNewPassword()
        {
            Console.WriteLine("Please enter URL of a site for your password: ");
            var readSite = Console.ReadLine();
            Console.WriteLine("Now please enter your password: ");
            var readPassword = Console.ReadLine();

            var password = new Password();

            CheckIsInputFilled(readSite);
            CheckIsInputFilled(readPassword);

            _service.CheckIsSiteAvailable(readSite);

            password.SiteOfPassword = readSite;
            password.PasswordString = readPassword;

            passwords.Add(password.SiteOfPassword, password.PasswordString);
            Console.WriteLine("Password added correctly");
        }

        public void RemovePassword()
        {
            Console.WriteLine("Enter a site of password that you want to delete: ");
            var readSite = Console.ReadLine();

            CheckIsInputFilled(readSite);

            if (passwords.ContainsKey(readSite) == false)
            {
                throw new Exception("You have not saved password for this site");
            }

            Console.WriteLine("Enter a password that you want to delete: ");
            var readPassword = Console.ReadLine();

            CheckIsInputFilled(readPassword);

            var removed = passwords.Remove(readSite, out readPassword);

            if (removed == false)
            {
                Console.WriteLine("Password is incorrect");
            }
        }

        public void ChangePassword()
        {
            Console.WriteLine("Enter a site of password that you want to change: ");
            var readSite = Console.ReadLine();

            CheckIsInputFilled(readSite);

            if (passwords.ContainsKey(readSite) == false)
            {
                throw new Exception("You have not saved password for this site");
            }

            Console.WriteLine("Enter a password that you want to delete: ");
            var readPassword = Console.ReadLine();

            CheckIsInputFilled(readPassword);

            Console.WriteLine("Enter new password: ");
            var newReadPassword = Console.ReadLine();

            CheckIsInputFilled(newReadPassword);

            for (int i = 0; i < passwords.Count; i++)
            {
                if (passwords.ContainsValue(readPassword))
                {
                    var removed = passwords.Remove(readSite, out readPassword);

                    if (removed == false)
                    {
                        throw new Exception("Incorrect password");
                    }

                    var newPassword = new Password
                    {
                        SiteOfPassword = readSite,
                        PasswordString = newReadPassword
                    };

                    passwords.Add(readSite, newReadPassword);
                }
            }
        }

        public void GetAllPasswordsWithSites()
        {
            int i = 1;
            Console.WriteLine("ID|SITE|PASSWORD");
            foreach (var password in passwords)
            {
                Console.WriteLine($"{i}|{password.Key}|{password.Value}");
                i++;
            }
        }

        public void FindPasswordOnWrittenSite()
        {
            Console.WriteLine("Enter a site name: ");
            var readSite = Console.ReadLine();

            CheckIsInputFilled(readSite);

            bool isPageFound = false;

            foreach (var password in passwords)
            {
                if (passwords.ContainsKey(readSite))
                {
                    Console.WriteLine($"A password for this site is: {password.Value}");
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

