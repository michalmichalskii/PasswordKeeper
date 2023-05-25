using PasswordKeeper.Domain.Common;

namespace PasswordKeeper.Domain.Entity
{
    public class MenuAction : BaseEntity
    {
        public string Action { get; set; }

        public MenuAction(int id, string action)
        {
            Id = id;
            Action = action;
        }
    }
}