using PasswordKeeper.App.Abstarct;
using PasswordKeeper.App.Concrete;
using PasswordKeeper.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PasswordKeeper.App.Managers
{
    public class UserDataManager
    {
        private readonly MenuActionService _menuActionService;
        private IService<UserDataModel> _userDataService;
        private WebManager _webManager;
        public UserDataManager(MenuActionService actionService, IService<UserDataModel> userDataService)
        {
            _menuActionService = actionService;
            _webManager = new WebManager();
            _userDataService = userDataService;
        }

        public void InitializeSomePasswords()
        {
            _userDataService.AddItem(new UserDataModel(1, "wp.pl", "mich@wp.pl", "Password1"));
            _userDataService.AddItem(new UserDataModel(2, "polska.pl", "mich@polska.pl", "Password2"));
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
        private bool IsPasswordCorrect(string readPassword)
        {
            if (readPassword.Length < 6)
            {
                Console.WriteLine("Password has to have minimum 6 characters");
                Console.WriteLine("Enter correct password:");
                return false;
            }
            else
                return true;
        }
        public List<UserDataModel> GetPasswordsList()
        {
            return _userDataService.Items;
        }
        public UserDataModel GetUserById(int id)
        {
            var item = _userDataService.GetItemById(id);
            return item;
        }
        public UserDataModel AddNewUserData()
        {
            bool isFilled = false, isCorrectPass = false;
            string readSite = "", readEmail = "", readPassword = "";

            while (!isFilled)
            {
                Console.WriteLine("Please enter URL of a site for your password: ");
                readSite = Console.ReadLine();
                Console.WriteLine("Please enter your Email for this site: ");
                readEmail = Console.ReadLine();
                Console.WriteLine("Now please enter your password: (min length is 6 chars)");
                do
                {
                    readPassword = Console.ReadLine();
                    isCorrectPass = IsPasswordCorrect(readPassword);
                } while (!isCorrectPass);

                isFilled = CheckIsInputFilled(readEmail, readSite, readPassword);
            }

            if (!_webManager.CheckIsSiteAvailable(readSite))
            {
                Console.WriteLine("This site doesn't exist");
                return null;
            }

            int lastId = _userDataService.GetLastId();
            var userData = new UserDataModel(lastId + 1, readSite, readEmail, readPassword);

            try
            {
                _userDataService.AddItem(userData);
            }
            catch (Exception)
            {
                Console.WriteLine("You already have password for this site");
                return null;
            }

            Console.WriteLine("Password added correctly");
            return userData;
        }
        private UserDataModel GetUserBySite()
        {
            string readSite = Console.ReadLine();

            if (!_userDataService.Items.Any(x => x.Site == readSite))
            {
                Console.WriteLine("You have not saved password for this site");
                return null;
            }

            foreach (var userModel in _userDataService.Items)
            {
                if (userModel.Site == readSite)
                {
                    return userModel;
                }
            }

            return null;
        }

        public UserDataModel RemoveUserData()
        {
            Console.WriteLine("Enter a site of password that you want to delete: ");

            UserDataModel userDataToDelete = GetUserBySite();

            if (userDataToDelete == null)
                return null;

            _userDataService.Items.Remove(userDataToDelete);
            Console.WriteLine("Data deleted correctly");
            return userDataToDelete;
        }

        public UserDataModel ChangePassword()
        {
            var isFilled = false;
            string readSite = "", readEmail = "", readPassword = "";

            Console.WriteLine("Enter a site of password that you want to change: ");

            UserDataModel userModelToChange = GetUserBySite();
            if (userModelToChange == null)
            {
                return null;
            }

            while (isFilled == false)
            {

                Console.WriteLine("Enter a email for site where you want to change a password: ");
                readEmail = Console.ReadLine();

                Console.WriteLine("Enter an old password: ");
                readPassword = Console.ReadLine();

                isFilled = CheckIsInputFilled(readEmail, readPassword);
            }

            if (userModelToChange.EmailOrLogin != readEmail || userModelToChange.PasswordString != readPassword)
            {
                Console.WriteLine("Email/Login or password are incorrect");
                return null;
            }

            Console.WriteLine("Enter a new password:");
            string readNewPassword = Console.ReadLine();

            userModelToChange.PasswordString = readNewPassword;

            Console.WriteLine("operation succeeded");
            return userModelToChange;
        }

        public void GetAllPasswordsWithSites()
        {
            Console.WriteLine("ID|SITE|EMAIL|PASSWORD");
            foreach (var password in _userDataService.Items)
            {
                Console.WriteLine($"{password.Id}|{password.Site}|{password.EmailOrLogin}|{password.PasswordString}");
            }
        }

        public void FindPasswordOnWrittenSite()
        {
            Console.WriteLine("Enter a site name: ");

            UserDataModel userDataModel = GetUserBySite();
            Console.WriteLine($"A password for this site is: {userDataModel.PasswordString}");
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
