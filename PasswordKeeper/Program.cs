using PasswordKeeper.App.Concrete;
using PasswordKeeper.App.Managers;
using PasswordKeeper.Domain.Entity;
namespace PasswordKeeper
{
    public class Program
    {

        //TODO - 1.add something to check is Password strong
        //TODO - 2.user has to get info if something goes wrong in incorrect try of password change

        static void Main(string[] args)
        {
            MenuActionService menuActionService = new MenuActionService();
            UserDataService userDataService = new UserDataService();
            UserDataManager userDataManager = new UserDataManager(menuActionService,userDataService);
            TxtFileManager txtFileService = new TxtFileManager(userDataManager);

            userDataManager.InitializeSomePasswords();

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
                        userDataManager.RemoveUserData();
                        break;
                    case ConsoleKey.D3:
                        userDataManager.ChangePassword();
                        break;
                    case ConsoleKey.D4:
                        userDataManager.GetAllPasswordsWithSites(); //in future only for admin
                        break;
                    case ConsoleKey.D5:
                        txtFileService.SaveToTxtFile();
                        break;
                    case ConsoleKey.D6:
                        userDataManager.FindPasswordOnWrittenSite(); //in future only for admin
                        break;
                    case ConsoleKey.D7:
                        userDataManager.GenerateRandomPassword();
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