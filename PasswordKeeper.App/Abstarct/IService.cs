using PasswordKeeper.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordKeeper.App.Abstarct
{
    public interface IService<T> where T : BaseEntity
    {
        List<T> Items { get; set; }

        List<T> GetAllItems();

        T GetItemById(int id);

        int AddItem(T item);

        bool DeleteItem(T item);

        bool DeleteItemById(int id);

        int GetLastId();
    }
}
