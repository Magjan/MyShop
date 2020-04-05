using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
     public  class inMemoryRepository<T> where T : BaseEntity
    {

        ObjectCache cache = MemoryCache.Default;
        List<T> items;
        string className;

        public inMemoryRepository() {
            // Everytime we call inMemoryRepository this line of code will store name of called class Name
            className = typeof(T).Name;
            items = cache[className] as List<T>;
            
            if (items==null) {
                items = new List<T>();  
            }
        }

        public void Commit() {
            cache[className] = items;
        }


        public void Insert(T t) {
            items.Add(t);
                   
        }

        public void Update(T t) {
            T updateedT = items.Find(i=>i.Id==t.Id);

            if (updateedT!=null) {
                updateedT = t;
            }
            else {
                throw new Exception("Cannot find an item");
            }

        }

        public T Find(string Id) {
            T t = items.Find(i=>i.Id==Id);
            if (t != null) {
                return t;
            }
            else {
                throw new Exception("Cannot find an item");
            }
        }


        public IQueryable<T> Collection() {
            return items.AsQueryable();
        }


        public void Delete(string Id) {
            T t = items.Find(i=>i.Id==Id);
            
            if (t!=null) {
                items.Remove(t);
            }
            else { 
                throw new Exception(className+" cannot be found ");
            }
        }

    }
}












