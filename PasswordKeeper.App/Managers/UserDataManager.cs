using ConsoleTables;
using PasswordKeeper.App.Abstarct;
using PasswordKeeper.App.Concrete;
using PasswordKeeper.App.Settings;
using PasswordKeeper.Cypher;
using PasswordKeeper.Domain.Entity;
using PasswordKeeper.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PasswordKeeper.App.Managers
{
    public class UserDataManager
    {
        private readonly MenuActionService _menuActionService;
        private UserDataService _userDataService;
        private WebService _webService;
        public UserDataManager(MenuActionService actionService, UserDataService userDataService)
        {
            _menuActionService = actionService;
            _webService = new WebService();
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

            var UserAssignedToSite = _userDataService.GetAllUsersFromJson().Where(u => u.Site == site).FirstOrDefault();

            if (UserAssignedToSite != null)
            {
                Console.WriteLine("You already have saved data for this site");
                return null;
            }
            if (!_webService.CheckIsSiteAvailable(site))
            {
                Console.WriteLine("This site doesn't exist");
                return null;
            }
            return site;
        }

        private bool IsValidEmail(string email)
        {
            EmailAddressAttribute emailAddressAttribute = new EmailAddressAttribute();
            if (!emailAddressAttribute.IsValid(email))
            {
                Console.WriteLine("Incorrect email format");
                return false;
            }
            return true;
        }

        private string GetCorrectEmail(string prompt)
        {
            bool isCorrectEmail;
            string email;
            do
            {
                email = GetUserNotNullStringInput(prompt);
                isCorrectEmail = IsValidEmail(email);
            } while (!isCorrectEmail);
            return email;
        }

        private User GetUserBySite()
        {
            string site = GetUserNotNullStringInput("Enter a site name: ");
            if (!_userDataService.GetAllUsersFromJson().Any(x => x.Site == site))
            {
                Console.WriteLine("You have not saved password for this site");
                return null;
            }

            var user = _userDataService.GetAllUsersFromJson().Where(p => p.Site == site).FirstOrDefault();

            return user;
        }

        public User AddNewUserData()
        {
            _userDataService.Items = _userDataService.GetAllUsersFromJson();
            int lastId = _userDataService.GetLastIdFromJson();
            string Site;
            do
            {
                Site = GetUniqueSite("Enter a site name: ");
            } while (Site == null);

            string newUserEmailOrLogin = GetCorrectEmail("Enter yor email: ");
            string newUserPasswordString = GetCorrectUserPassword("Enter a password: ");
            newUserPasswordString = EncryptionsMaker.EncryptPlainTextToCipherText(newUserPasswordString);
            var newUser = new User(lastId + 1, Site, newUserEmailOrLogin, newUserPasswordString);

            try
            {
                _userDataService.AddItem(newUser);
            }
            catch (Exception)
            {
                Console.WriteLine("You already have password for this site");
                return null;
            }

            FilesHelper.SaveData(_userDataService.Items, "passwords.json");

            Console.WriteLine("Password added correctly");
            return newUser;
        }

        public int RemoveUserById()
        {
            DisplayLimitedTable();
            _userDataService.Items = _userDataService.GetAllUsersFromJson();

            Console.WriteLine("Enter an id that you want to delete: ");
            var readId = int.Parse(Console.ReadLine());
            if (!_userDataService.Items.Where(u => u.Id == readId).Any())
            {
                Console.WriteLine("\nIncorrect id");
                return 0;
            }

            if (!_userDataService.DeleteItemById(readId))
                return 0;

            FilesHelper.SaveData(_userDataService.Items, "passwords.json");

            return readId;
        }

        public User ChangePasswordByUserDataId()
        {
            DisplayLimitedTable();
            _userDataService.Items = _userDataService.GetAllUsersFromJson();

            Console.WriteLine("Enter an id that you want to change: ");
            var readId = int.Parse(Console.ReadLine());
            if (!_userDataService.Items.Where(u => u.Id == readId).Any())
            {
                Console.WriteLine("\nIncorrect id");
                return null;
            }
            string passwordString = GetUserNotNullStringInput("Enter your password: ");
            string encryptedPasswordString = EncryptionsMaker.EncryptPlainTextToCipherText(passwordString);

            User user = _userDataService.GetItemById(readId);

            if (user.PasswordString != encryptedPasswordString)
            {
                Console.WriteLine("Email/Login or password are incorrect");
                return null;
            }

            string readNewPassword = GetCorrectUserPassword("Enter a new password: ");

            user.PasswordString = EncryptionsMaker.EncryptPlainTextToCipherText(readNewPassword);

            FilesHelper.SaveData(_userDataService.Items, "passwords.json");

            Console.WriteLine("operation succeeded");
            return user;
        }

        public ConsoleTable DisplayLimitedTable()
        {
            var users = _userDataService.GetAllUsersFromJson();

            int i = 0;

            ConsoleTable table;

            table = new ConsoleTable("ID", "SITE", "EMAIL", "PASSWORD");

            if (users != null)
            {
                foreach (var user in users)
                {
                    table.AddRow(user.Id, user.Site, user.EmailOrLogin, "**********");
                    i++;
                }
            }

            Console.WriteLine(table.ToString());

            return table;
        }

        public int DisplayFullTable(ConsoleTable limitedTable)
        {
            var users = _userDataService.GetAllUsersFromJson();

            if (users.Count == 0)
                return 0;

            Console.WriteLine("\nIf you want to see full table type an admin password, otherwise just click Enter button: ");
            string password = Console.ReadLine();
            if (!string.IsNullOrEmpty(password))
            {
                if (ValidAdminPassword(password))
                {
                    if (users != null)
                    {
                        limitedTable.Rows.Clear();
                        foreach (var user in users)
                        {
                            limitedTable.AddRow(user.Id, user.Site, user.EmailOrLogin, DecryptionMaker.DecryptCipherTextToPlainText(user.PasswordString));
                        }
                    }
                    Console.WriteLine(limitedTable.ToString());
                }
                else
                {
                    Console.WriteLine("Invalid password");
                }
            }
            return users.Count;
        }

        private bool ValidAdminPassword(string password)
        {
            return EncryptionsMaker.EncryptPlainTextToCipherText(password) == FilesHelper.LoadData<AppConfig>("appsettings.json").AdminPassword;
        }

        public void FindPasswordBySite()
        {
            Console.WriteLine("Enter an admin password: ");
            string password = Console.ReadLine();
            if (ValidAdminPassword(password))
            {
                User userDataModel = GetUserBySite();
                if (userDataModel != null)
                {
                    Console.WriteLine($"A password for this site is: {DecryptionMaker.DecryptCipherTextToPlainText(userDataModel.PasswordString)}");
                }
            }
        }
    }
}
