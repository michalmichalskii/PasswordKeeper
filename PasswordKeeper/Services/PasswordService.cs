namespace PasswordKeeper.Services
{
    public class PasswordService
    {
        private WebService _service;

        private Dictionary<string, UserDataModel> _passwords;

        public PasswordService(WebService service)
        {
            _passwords = new Dictionary<string, UserDataModel>();
            _service = service;
        }

        private bool CheckIsInputFilled(params string[] checkedInput)
        {
            for (int i = 0; i < checkedInput.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(checkedInput[i]))
                {
                    Console.WriteLine("You cannot left blanks");
                    return false;
                }
            }

            return true;
        }

        public Dictionary<string, UserDataModel> GetPasswordsDictionary()
        {
            return _passwords;
        }

        public void AddNewPassword()
        {
            var isFilled = false;
            string readSite = "", readEmail = "", readPassword = "";

            while (isFilled == false)
            {
                Console.WriteLine("Please enter URL of a site for your password: ");
                readSite = Console.ReadLine();
                Console.WriteLine("Please enter your Email for this site: ");
                readEmail = Console.ReadLine();
                Console.WriteLine("Now please enter your password: ");
                readPassword = Console.ReadLine();

                isFilled = CheckIsInputFilled(readEmail, readSite, readPassword);
            }

            var userData = new UserDataModel();

            var isAvailable = _service.CheckIsSiteAvailable(readSite);

            userData.EmailOrLogin = readEmail;
            userData.PasswordString = readPassword;

            _passwords.Add(readSite, userData);

            Console.WriteLine("Password added correctly");
        }

        public void RemovePassword()
        {
            var isFilled = false;
            string readSite = "", readEmail = "", readPassword = "";

            while (isFilled == false)
            {
                Console.WriteLine("Enter a site of password that you want to delete: ");
                readSite = Console.ReadLine();

                if (_passwords.ContainsKey(readSite) == false)
                {
                    throw new Exception("You have not saved password for this site");
                }

                Console.WriteLine("Enter a email for site that you want to delete: ");
                readEmail = Console.ReadLine();
                Console.WriteLine("Enter a password that you want to delete: ");
                readPassword = Console.ReadLine();

                isFilled = CheckIsInputFilled(readSite, readEmail, readPassword);
            }

            UserDataModel userData = new UserDataModel()
            {
                EmailOrLogin = readEmail,
                PasswordString = readPassword
            };

            var removed = _passwords.Remove(readSite, out userData);

            if (removed == false)
            {
                Console.WriteLine("Password or email is incorrect");
            }

        }

        public void ChangePassword()
        {
            var isFilled = false;
            string readSite = "", readEmail = "", readPassword = "";

            while (isFilled == false)
            {
                Console.WriteLine("Enter a site of password that you want to change: ");
                readSite = Console.ReadLine();

                if (_passwords.ContainsKey(readSite) == false)
                {
                    throw new Exception("You have not saved password for this site");
                }

                Console.WriteLine("Enter a email for site where you want to change a password: ");
                readEmail = Console.ReadLine();

                Console.WriteLine("Enter an old password: ");
                readPassword = Console.ReadLine();

                isFilled = CheckIsInputFilled(readSite, readEmail, readPassword);
            }

            var oldUserData = new UserDataModel()
            {
                EmailOrLogin = readEmail,
                PasswordString = readPassword
            };

            for (int i = 0; i < _passwords.Count; i++)
            {
                if (_passwords.TryGetValue(readSite, out oldUserData))
                {
                    var removed = _passwords.Remove(readSite, out oldUserData);

                    if (removed == false)
                    {
                        throw new Exception("Incorrect password");
                    }
                }
            }
            var newReadPassword = "";
            var isFillednewPassword = false;
            while (isFillednewPassword == false)
            {
                Console.WriteLine("Enter a new password: ");
                newReadPassword = Console.ReadLine();

                isFillednewPassword = CheckIsInputFilled(newReadPassword);
            }

            var newUserData = new UserDataModel()
            {
                EmailOrLogin = readEmail,
                PasswordString = newReadPassword
            };

            _passwords.Add(readSite, newUserData);
        }

        public void GetAllPasswordsWithSites()
        {
            int i = 1;
            Console.WriteLine("ID|SITE|EMAIL|PASSWORD");
            foreach (var password in _passwords)
            {
                Console.WriteLine($"{i}|{password.Key}|{password.Value.EmailOrLogin}|{password.Value.PasswordString}");
                i++;
            }
        }

        public void FindPasswordOnWrittenSite()
        {
            var isFilled = false;
            var readSite = "";

            while (isFilled == false)
            {
                Console.WriteLine("Enter a site name: ");
                readSite = Console.ReadLine();

                isFilled = CheckIsInputFilled(readSite);
            }

            bool isPageFound = false;

            foreach (var password in _passwords)
            {
                if (_passwords.ContainsKey(readSite))
                {
                    Console.WriteLine($"A password for this site is: {password.Value.PasswordString}");
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

                var random = new Random();
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