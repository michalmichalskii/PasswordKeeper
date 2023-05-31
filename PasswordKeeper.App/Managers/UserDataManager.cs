using ConsoleTables;
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
        private IService<User> _userDataService;
        private WebService _webManager;
        public UserDataManager(MenuActionService actionService, IService<User> userDataService)
        {
            _menuActionService = actionService;
            _webManager = new WebService();
            _userDataService = userDataService;
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
        private int GetUserIntKeyInput()
        {
            var key = Console.ReadKey();
            var readId = key.KeyChar.ToString();
            return int.Parse(readId);
        }
        private string GetUserNotNullStringInput(string prompt)
        {
            string retrievedString = "";
            bool isFilled = false;
            while (!isFilled)
            {
                Console.WriteLine(prompt);
                retrievedString = Console.ReadLine();
                isFilled = CheckIsInputFilled(retrievedString);
            }
            return retrievedString;
        }
        private bool IsPasswordCorrect(string readPassword)
        {
            if (readPassword.Length < 6)
            {
                Console.WriteLine("Password has to have minimum 6 characters");
                return false;
            }
            else
                return true;
        }
        private string GetCorrectUserPassword(string prompt)
        {
            bool isCorrectPass;
            string password;
            do
            {
                password = GetUserNotNullStringInput(prompt);
                isCorrectPass = IsPasswordCorrect(password);
            } while (!isCorrectPass);
            return password;
        }
        private string GetUniqueSite(string prompt)
        {
            string site = GetUserNotNullStringInput(prompt);
            foreach (var item in _userDataService.Items)
            {
                if (item.Site == site)
                {
                    Console.WriteLine("You already have saved data for this site");
                    return null;
                }
            }
            return site;
        }
        private string GetCorrectEmail(string prompt)
        {
            bool isCorrectEmail;
            string email;

            do
            {
                email = GetUserNotNullStringInput(prompt);
                isCorrectEmail = IsEmailUniqueAndCorrect(email);
            } while (!isCorrectEmail);
            return email;
        }
        private bool IsEmailUniqueAndCorrect(string email)
        {
            if (!email.Contains('@') && !email.Contains('.'))
            {
                Console.WriteLine("Incorrect email format");
                return false;
            }
            return true;
        }

        public List<User> GetPasswordsList()
        {
            return _userDataService.Items;
        }
        public User GetUserById(int id)
        {
            var item = _userDataService.GetItemById(id);
            return item;
        }
        public User AddNewUserData()
        { 
            int lastId = _userDataService.GetLastId();
            string Site = GetUniqueSite("Enter a site name: ");
            if (Site == null)
                return null;
            string newUserEmailOrLogin = GetCorrectEmail("Enter yor email: ");
            string newUserPasswordString = GetCorrectUserPassword("Enter a password: ");

            User newUser = new User(lastId+1, Site, newUserEmailOrLogin, newUserPasswordString);

            try
            {
                _userDataService.AddItem(newUser);
            }
            catch (Exception)
            {
                Console.WriteLine("You already have password for this site");
                return null;
            }

            Console.WriteLine("Password added correctly");
            return newUser;
        }

        private User GetUserBySite()
        {
            string site = GetUserNotNullStringInput("Enter a site name: ");
            if (!_userDataService.Items.Any(x => x.Site == site))
            {
                Console.WriteLine("You have not saved password for this site");
                return null;
            }

            foreach (var userModel in _userDataService.Items)
            {
                if (userModel.Site == site)
                {
                    return userModel;
                }
            }

            return null;
        }

        public int RemoveUserById()
        {
            GetAllPasswordsWithSites(false);
            Console.WriteLine("Enter an id that you want to delete: ");
            var readId = GetUserIntKeyInput();
            if(readId > _userDataService.Items.Count)
            {
                Console.WriteLine("\nIncorrect id");
                return 0;
            }

            if (!_userDataService.DeleteItemById(readId))
                return 0;

            return readId;
        }
        public User ChangePasswordByUserDataId()
        {
            GetAllPasswordsWithSites(false);
            Console.WriteLine("Enter an id that you want to change: ");
            var readId = GetUserIntKeyInput();
            if (readId > _userDataService.Items.Count)
            {
                Console.WriteLine("\nIncorrect id");
                return null;
            }
            string emailOrLogin = GetUserNotNullStringInput("Enter yor email: ");
            string passwordString = GetUserNotNullStringInput("Enter your password: ");

            var user = GetUserById(readId);

            if (user.EmailOrLogin != emailOrLogin || user.PasswordString != passwordString)
            {
                Console.WriteLine("Email/Login or password are incorrect");
                return null;
            }

            string readNewPassword = GetCorrectUserPassword("Enter a new password: ");

            user.PasswordString = readNewPassword;

            Console.WriteLine("operation succeeded");
            return user;
        }

        public void GetAllPasswordsWithSites(bool showAll = true)
        {
            ConsoleTable table;
            if (showAll)
                table = new ConsoleTable("ID", "SITE", "EMAIL", "PASSWORD");
            else
                table = new ConsoleTable("ID", "SITE");

            foreach (var password in _userDataService.Items)
            {
                if (showAll)
                    table.AddRow(password.Id, password.Site, password.EmailOrLogin, password.PasswordString);
                else
                    table.AddRow(password.Id, password.Site);
            }
            Console.WriteLine(table.ToString());
        }

        public void FindPasswordOnWrittenSite()
        {
            User userDataModel = GetUserBySite();
            Console.WriteLine($"A password for this site is: {userDataModel.PasswordString}");
        }
    }
}
