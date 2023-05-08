using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordKeeper
{
    public class MenuActionService
    {
        private List<MenuAction> menuActions;

        public MenuActionService()
        {
            menuActions = new List<MenuAction>();
        }

        public void AddActionToMenu(int id, string action)
        {
            MenuAction menuAction = new MenuAction() { Id = id, Action = action };
            menuActions.Add(menuAction);
        }

        public List<MenuAction> GetMenuActions()
        {
            var results = new List<MenuAction>();

            foreach (var menuAction in menuActions)
            {
                results.Add(menuAction);
            }
            return results;
        }
    }
}
