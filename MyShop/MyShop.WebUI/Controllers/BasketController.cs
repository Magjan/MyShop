using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class BasketController : Controller
    {
        IRepository<Customer> customerContext;
        IBasketService basketService;
        IOrderService orderService;


        public BasketController(IBasketService BasketService, IOrderService OrderService, IRepository<Customer> CustomerContext) {
            this.basketService = BasketService;
            this.orderService = OrderService;
            this.customerContext = CustomerContext;
        }

        // GET: Basket
        public ActionResult Index()
        {
            var model = basketService.GetBasketItems(this.HttpContext);
            return View(model);
        }



        public ActionResult AddToBasket(string Id) {
            basketService.AddToBasket(this.HttpContext, Id);
            return RedirectToAction("Index");
        }


        public ActionResult RemoveFromBasket(string Id)
        {
            basketService.RemoveFrombasket(this.HttpContext, Id);
            return RedirectToAction("Index");
        }


        public PartialViewResult BasketSummary()
        {
            var basketSummary = basketService.GetBasketSummary(this.HttpContext);
            return PartialView(basketSummary);
        }


        public ActionResult Checkout() {
            // get current customer
            Customer customer = customerContext
                .Collection().
                FirstOrDefault(c=>c.Email==User.Identity.Name);

            if (customer != null)
            {
                Order order = new Order()
                {
                    Email = customer.Email,
                    City = customer.City,
                    FisrtName = customer.FisrtName,
                    LastName = customer.LastName,
                    State = customer.State,
                    Street = customer.Street,
                    ZipCode = customer.ZipCode
                };

                return View(order);
            }
            else {
                return RedirectToAction("Login", "Account");
            }

           
        }
        [HttpPost]
        [Authorize]
        public ActionResult Checkout(Order order)
        {

            var basketItems = basketService.GetBasketItems(this.HttpContext);
            order.OrderStatus = "Order created";
            order.Email = User.Identity.Name;

            order.OrderStatus = "Payment processed";
            orderService.CreateOrder(order, basketItems);

            basketService.ClearBasket(this.HttpContext);

            return RedirectToAction("Thankyou", new {  OrderId=order.Id });
        }

        public ActionResult Thankyou(string OrderId )
        {
            ViewBag.OrderId = OrderId;
            return View();
        }

    }
}