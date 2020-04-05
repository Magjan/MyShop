﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;


namespace MyShop.WebUI.Controllers
{
    public class ProductCategoryManagerController:Controller
    {

        ProductCategoryRepository context;

        public ProductCategoryManagerController()
        {
            context = new ProductCategoryRepository();
        }


        // GET: ProductManager
        public ActionResult Index()
        {

            List<ProductCategory> productcategories = context.Collection().ToList();
            return View(productcategories);
        }


        public ActionResult Create()
        {

            ProductCategory productCategory = new ProductCategory();
            return View(productCategory);
        }

        [HttpPost]
        public ActionResult Create(ProductCategory productCategory)
        {

            if (!ModelState.IsValid)
            {
                return View(productCategory);
            }
            else
            {

                context.Insert(productCategory);
                context.Commit();


                return RedirectToAction("Index");

            }


        }


        public ActionResult Edit(string Id)
        {

            ProductCategory productCategory = context.Find(Id);

            if (productCategory == null)
            {
                return HttpNotFound();
            }

            return View(productCategory);

        }

        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory, string Id)
        {

            ProductCategory productCategoryToUpdate = context.Find(Id);


            if (productCategoryToUpdate == null)
            {

                return HttpNotFound();

            }
            else
            {

                if (!ModelState.IsValid)
                {
                    return View(productCategoryToUpdate);
                }
                else
                {

                    productCategoryToUpdate.Category = productCategory.Category;
        


                    context.Commit();
                    return RedirectToAction("Index");
                }

            }

        }


        public ActionResult Delete(string Id)
        {

            ProductCategory productCategoryForDelete = context.Find(Id);

            if (productCategoryForDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productCategoryForDelete);
            }

        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {

            ProductCategory productCategoryToDelete = context.Find(Id);

            if (productCategoryToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.DeleteProduct(Id);
                context.Commit();

                return RedirectToAction("Index");
            }
        }

    }
}