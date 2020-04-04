using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using MyShop.Core.Models;

namespace MyShop.DataAccess.InMemory
{
    public  class ProductRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<Product> products;

        public ProductRepository() {

            products = cache["products"] as List<Product>;

            if (products==null) {
                products = new List<Product>();
            }

        }

        // set products to cache
        public void Commit() {
            cache["products"] = products;                   
        }

        public void Insert(Product product) {
            products.Add(product);
        }


        public void Update(Product product)
        {
            Product productToUpdate = products.Find(p=>p.Id== product.Id);

            if (productToUpdate==null) {
                throw new Exception("Product cannot be found");
            }

            productToUpdate = product;
            Commit();
        }


        public Product Find(string  Id ) {
            Product product = products.Find(p=>p.Id==Id);

            if (product==null) {
                throw new Exception("Product cannot be found");
            }
            return product;
        }

        public IQueryable<Product> Collection() { 
            
            return products.AsQueryable();
        }


        public void DeleteProduct(string Id) {
            Product productDelete = products.Find(p=>p.Id==Id);
            if (productDelete==null) {
                throw new Exception("Product cannot be found");
            }

            products.Remove(productDelete);

           // Commit();

        }


    }
}
