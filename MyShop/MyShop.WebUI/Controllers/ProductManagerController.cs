﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModel;
using MyShop.DataAccess.InMemory;

namespace MyShop.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductManagerController : Controller
    {

        IRepository<Product> context;
          IRepository<ProductCategory>  productCategories; 

        public ProductManagerController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext) {
             this.context = productContext;
             this.productCategories = productCategoryContext;
        }


        // GET: ProductManager
        public ActionResult Index()
        {

            List<Product> products = context.Collection().ToList();
            return View(products);
        }


        public ActionResult Create() {

            ProductManagerViewModel viewModel = new ProductManagerViewModel();


            // this isn third class which store Product and ProductCategory
            viewModel.Product = new Product();
            viewModel.ProductCategories = productCategories.Collection();

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {

            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else {

                // save image file
                if (file!=null) {

                    product.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") +product.Image);

                }


                context.Insert(product);
                context.Commit();


                return RedirectToAction("Index");

            }
            
            
        }


        public ActionResult Edit(string Id) {

            Product product = context.Find(Id);

            if (product==null) {
                return HttpNotFound();
            }

            ProductManagerViewModel viewModel = new ProductManagerViewModel();

            // this isn third class which store Product and ProductCategory
            viewModel.Product = product;
            viewModel.ProductCategories = productCategories.Collection();

            return View(viewModel);

        }

        [HttpPost]
        public ActionResult Edit(Product product, string Id, HttpPostedFileBase file) {

            Product productToUpdate = context.Find(Id);


            if (productToUpdate==null) {

                return HttpNotFound();

            }
            else {

                if (!ModelState.IsValid)
                {
                    return View(product);
                }
                else
                {

                    // save image file
                    if (file != null)
                    {

                        productToUpdate.Image = product.Id + Path.GetExtension(file.FileName);
                        file.SaveAs(Server.MapPath("//Content//ProductImages//") + productToUpdate.Image);

                    }


                    productToUpdate.Category = product.Category;
                    productToUpdate.Description = product.Description;
                 
                    productToUpdate.Name = product.Name;
                    productToUpdate.Price = product.Price;
                   
                   
                    context.Commit();
                    return RedirectToAction("Index");
                }

            }

        }


        public ActionResult Delete(string Id) {

            Product productForDelete = context.Find(Id);

            if (productForDelete==null) {
                return HttpNotFound();
            }
            else {
                return View(productForDelete);
            }

        }

       [HttpPost]
       [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id) {

            Product productToDelete = context.Find(Id);

            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);
                context.Commit();

                return RedirectToAction("Index");
            }
        }

    }
}