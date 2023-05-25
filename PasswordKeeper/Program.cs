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
            WebManager webService = new WebManager();
            PasswordService passwordService = new PasswordService();
            PasswordManager passwordManager = new PasswordManager(menuActionService);
            TxtFileManager txtFileService = new TxtFileManager(passwordManager);

            passwordManager.InitializeSomePasswords();

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
                        passwordManager.AddNewUserData();
                        break;
                    case ConsoleKey.D2:
                        passwordManager.RemoveUserData();
                        break;
                    case ConsoleKey.D3:
                        passwordManager.ChangePassword();
                        break;
                    case ConsoleKey.D4:
                        passwordManager.GetAllPasswordsWithSites(); //in future only for admin
                        break;
                    case ConsoleKey.D5:
                        txtFileService.SaveToTxtFile();
                        break;
                    case ConsoleKey.D6:
                        passwordManager.FindPasswordOnWrittenSite(); //in future only for admin
                        break;
                    case ConsoleKey.D7:
                        passwordManager.GenerateRandomPassword();
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