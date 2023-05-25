using PasswordKeeper.App.Common;
using PasswordKeeper.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordKeeper.App.Concrete
{
    public class MenuActionService : BaseService<MenuAction>
    {
        public MenuActionService()
        {
            Initialize();
        }

        public void Initialize()
        {
            AddItem(new MenuAction(1,"Create a password and assign it to a website"));
            AddItem(new MenuAction(2, "Delete the password for an assigned website"));
            AddItem(new MenuAction(3, "Modify the password for an assigned website"));
            AddItem(new MenuAction(4, "Show all passwords with their corresponding websites"));
            AddItem(new MenuAction(5, "Save all passwords to a txt file //prototype"));
            AddItem(new MenuAction(6, "Find the password for a specific website"));
            AddItem(new MenuAction(7, "Generate a random password"));
            AddItem(new MenuAction(0, "Exit"));
        }
    }
}
