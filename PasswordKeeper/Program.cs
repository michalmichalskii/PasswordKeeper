using ConsoleTables;
using PasswordKeeper.App.Concrete;
using PasswordKeeper.App.Managers;
using PasswordKeeper.App.Settings;
using PasswordKeeper.Domain.Entity;

namespace PasswordKeeper
{
    public class Program
    {
        static void Main(string[] args)
        {
            var appConfig = new AppConfig();
            appConfig.SetConfig();

            var menuActionService = new MenuActionService();
            var userDataService = new UserDataService();
            var userDataManager = new UserDataManager(menuActionService, userDataService);

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
                        ConsoleTable limitedTable = userDataManager.DisplayLimitedTable();
                        userDataManager.DisplayFullTable(limitedTable);
                        break;
                    case ConsoleKey.D5:
                        userDataManager.FindPasswordBySite();
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