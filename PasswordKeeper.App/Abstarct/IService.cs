using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordKeeper.App.Abstarct
{
    public interface IService<T>
    {
        List<T> Items { get; set; }
        List<T> GetAllItems();
        T GetItemById(int id);
        int AddItem(T item);
        void DeleteItem(T item);
        int UpdateItem(T item);
        int GetLastId();
    }
}
