using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
     public class ProductCategoryRepository
    {

        ObjectCache cache = MemoryCache.Default;
        List<ProductCategory> productcategories;

        public ProductCategoryRepository()
        {

            productcategories = cache["productcategories"] as List<ProductCategory>;

            if (productcategories == null)
            {
                productcategories = new List<ProductCategory>();
            }

        }

        // set products to cache
        public void Commit()
        {
            cache["productcategories"] = productcategories;
        }

        public void Insert(ProductCategory productCategory)
        {
            productcategories.Add(productCategory);
        }


        public void Update(ProductCategory productCategory)
        {
            ProductCategory productCategoryToUpdate = productcategories.Find(p => p.Id == productCategory.Id);

            if (productCategoryToUpdate == null)
            {
                throw new Exception("ProductCategory cannot be found");
            }

            productCategoryToUpdate = productCategory;
            Commit();
        }


        public ProductCategory Find(string Id)
        {
            ProductCategory productCategory = productcategories.Find(p => p.Id == Id);

            if (productCategory == null)
            {
                throw new Exception("Product cannot be found");
            }
            return productCategory;
        }

        public IQueryable<ProductCategory> Collection()
        {

            return productcategories.AsQueryable();
        }


        public void DeleteProduct(string Id)
        {
            ProductCategory productCategoryDelete = productcategories.Find(p => p.Id == Id);
            if (productCategoryDelete == null)
            {
                throw new Exception("Product cannot be found");
            }

            productcategories.Remove(productCategoryDelete);

            // Commit();

        }

    }
}
