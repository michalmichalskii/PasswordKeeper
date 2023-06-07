using PasswordKeeper.App.Concrete;
using PasswordKeeper.App.Managers;
using PasswordKeeper.App.Settings;
using PasswordKeeper.Domain.Entity;

namespace PasswordKeeper
{
    public class Program
    {

        //TODO - 1.add something to check is Password strong
        static void Main(string[] args)
        {
            var menuActionService = new MenuActionService();
            var userDataService = new UserDataService();
            var jsonFileService = new JsonFileService(userDataService);

            var userDataManager = new UserDataManager(menuActionService, userDataService, jsonFileService);

            var appConfig = new AppConfig();
            appConfig.SetConfig();

            var chosenActionNumb = new ConsoleKeyInfo();
            while (chosenActionNumb.Key != ConsoleKey.D0)
            {
                Console.WriteLine("Choose an option: ");

                var menu = menuActionService.GetAllItems();

                for (int i = 0; i < menu.Count; i++)
                {
                    Console.WriteLine($"{menu[i].Id}. {menu[i].Action}");
                }

                chosenActionNumb = Console.ReadKey();

                Console.WriteLine();//line for some visibility

                switch (chosenActionNumb.Key)
                {
                    case ConsoleKey.D1:
                        userDataManager.AddNewUserData();
                        break;
                    case ConsoleKey.D2:
                        userDataManager.RemoveUserById();
                        break;
                    case ConsoleKey.D3:
                        userDataManager.ChangePasswordByUserDataId();
                        break;
                    case ConsoleKey.D4:
                        userDataManager.DisplayTableWithPasswords(); //in future only for admin
                        break;
                    case ConsoleKey.D5:
                        userDataManager.FindPasswordBySite(); //in future only for admin
                        break;
                    case ConsoleKey.D6:
                        var randomPassword = userDataService.GenerateRandomPassword();
                        Console.WriteLine(randomPassword);
                        break;
                    case ConsoleKey.D0:
                        Console.WriteLine("Goodbye");
                        break;
                    default:
                        Console.WriteLine("Incorrect choice");
                        break;
                }
                Console.WriteLine("\n--------------------------\n");
            }
        }


    }
}