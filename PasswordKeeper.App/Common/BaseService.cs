using PasswordKeeper.App.Abstarct;
using PasswordKeeper.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordKeeper.App.Common
{
    public class BaseService<T> : IService<T> where T : BaseEntity
    {
        public List<T> Items { get; set; }

        public BaseService()
        {
            Items = new List<T>();
        }
        public T GetItemById(int id)
        {
            var entity = Items.FirstOrDefault(p => p.Id == id);
            return entity;
        }
        public int GetLastId()
        {
            int lastId;
            if(Items.Any())
            {
                lastId = Items.OrderByDescending(x => x.Id).FirstOrDefault().Id;
            }
            else
            {
                lastId=0;
            }
            return lastId;
        }
        public int AddItem(T item)
        {
            Items.Add(item);
            return item.Id;
        }

        public void DeleteItem(T item)
        {
            Items.Remove(item);
        }

        public List<T> GetAllItems()
        {
            return Items;
        }

        public int UpdateItem(T item)
        {
            var entity = Items.FirstOrDefault(x => x.Id == item.Id);
            if(entity != null)
            {
                entity = item;
            }
            return entity.Id;
        }
    }
}
