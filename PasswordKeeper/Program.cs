using PasswordKeeper.Services;

namespace PasswordKeeper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var passwordService = new PasswordService();
            var txtFileService = new TxtFileService(passwordService);

            var chosenActionNumb = new ConsoleKeyInfo();
            while (chosenActionNumb.Key != ConsoleKey.D0)
            {
                Console.WriteLine("Choose an option: ");

                var menuActionService = new MenuActionService();
                menuActionService = Initialize(menuActionService);
                var menu = menuActionService.GetMenuActions();

                for (int i = 0; i < menu.Count; i++)
                {
                    Console.WriteLine($"{menu[i].Id}. {menu[i].Action}");
                }

                chosenActionNumb = Console.ReadKey();

                Console.WriteLine();//line for some visibility

                switch (chosenActionNumb.Key)
                {
                    case ConsoleKey.D1:
                        passwordService.AddNewPassword();
                        break;
                    case ConsoleKey.D2:
                        passwordService.RemovePassword();
                        break;
                    case ConsoleKey.D3:
                        passwordService.ChangePassword();
                        break;
                    case ConsoleKey.D4:
                        passwordService.GetAllPasswordsWithSites(); //in future only for admin
                        break;
                    case ConsoleKey.D5:
                        txtFileService.SaveToTxtFile();
                        break;
                    case ConsoleKey.D6:
                        passwordService.FindPasswordOnWrittenSite(); //in future only for admin
                        break;
                    case ConsoleKey.D7:
                        passwordService.GenerateRandomPassword();
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

        public static MenuActionService Initialize(MenuActionService menuActionService)
        {
            menuActionService.AddActionToMenu(1, "Create a password and assign it to a website");
            menuActionService.AddActionToMenu(2, "Delete the password for an assigned website");
            menuActionService.AddActionToMenu(3, "Modify the password for an assigned website");
            menuActionService.AddActionToMenu(4, "Show all passwords with their corresponding websites");
            menuActionService.AddActionToMenu(5, "Save all passwords to a txt file //prototype");
            menuActionService.AddActionToMenu(6, "Find the password for a specific website");
            menuActionService.AddActionToMenu(7, "Generate a random password");
            menuActionService.AddActionToMenu(0, "Exit");
            
            return menuActionService;
        }
    }
}