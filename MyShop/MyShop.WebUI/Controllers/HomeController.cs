using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class HomeController : Controller
    {

        IRepository<Product> context;
        IRepository<ProductCategory> productCategories;
        public HomeController(IRepository<Product> ProductContext, IRepository<ProductCategory> productCategoriesContext) {
            this.context = ProductContext;
            this.productCategories = productCategoriesContext;
        }

        public ActionResult Index(string category=null)
        {

            List<Product> products = context.Collection().ToList();
            List<ProductCategory> categories = productCategories.Collection().ToList();


            if (category == null) {
                 products = context.Collection().ToList();
            }
            else {
                products = context.Collection().Where(p => p.Category == category).ToList();
            }


            ProductListViewModel model = new ProductListViewModel();

            model.Products = products;
            model.ProductCategories = categories;

            return View(model);
        }


        public ActionResult Details(string Id) {
            Product product = context.Find(Id);

            if (product==null)
            {
                return HttpNotFound();

            }

            return View(product);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}