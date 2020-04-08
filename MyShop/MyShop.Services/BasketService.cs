using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModel;
using MyShop.DataAcess.MySQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Services
{
    public class BasketService : IBasketService
    {

        IRepository<Product> ProductContext;
        IRepository<Basket> BasketContext;
        IRepository<BasketItem> basketItemsContext;


        public const string basketSessionName = "eCommerceBasket";

        public BasketService(IRepository<Product> productContext,
            IRepository<Basket> basketContext, IRepository<BasketItem> BasketItemsContext) {
            this.ProductContext = productContext;
            this.BasketContext = basketContext;
            this.basketItemsContext = BasketItemsContext;
        }



        // we gonna read cookie to check have product or not  
        private Basket GetBasket(HttpContextBase httpContext, bool createifnull) {

            HttpCookie cookie = httpContext.Request.Cookies.Get(basketSessionName);




            Basket basket = new Basket();

            if (cookie != null)
            {
                string basketId = cookie.Value;

                if (!string.IsNullOrEmpty(basketId))
                {
                    basket = BasketContext.Find(basketId);
                }
                else
                {
                    if (createifnull)
                    {
                        basket = CreateNewBasket(httpContext);
                    }

                }

            }
            else {

                if (createifnull)
                {
                    basket = CreateNewBasket(httpContext);
                }

            }

            return basket;

        }


        private Basket CreateNewBasket(HttpContextBase httpContext)
        {
            Basket basket = new Basket();

            BasketContext.Insert(basket);
            BasketContext.Commit();

            HttpCookie cookie = new HttpCookie(basketSessionName);
            cookie.Value = basket.Id;
            cookie.Expires = DateTime.Now.AddDays(1);
            httpContext.Response.Cookies.Add(cookie);


            return basket;
        }


        public void AddToBasket(  HttpContextBase httpContext, string productId) {

            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
            {
                item = new BasketItem()
                {
                    BasketId = basket.Id,
                    ProductId = productId,
                    Quantity = 1
                };

                basket.BasketItems.Add(item);

            }
            else {
                item.Quantity = item.Quantity + 1;

            }

            BasketContext.Commit();

        }


        public void RemoveFrombasket(HttpContextBase httpContext, string itemId) {

            Basket basket = GetBasket(httpContext, true);

           

            ICollection<BasketItem> items = basketItemsContext.Collection().ToList();
            BasketItem basketItemToDelete = (from s in items
                           where s.Id == itemId
                           select s).FirstOrDefault<BasketItem>();

            if (basketItemToDelete != null) {
                items.Remove(basketItemToDelete);
                basketItemsContext.Delete(itemId);
                basketItemsContext.Commit();
                //basket.BasketItems.Remove(item);
                // BasketContext.Commit();
            }

        }



        public List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext) {

            Basket basket = GetBasket(httpContext, false);

            ICollection<BasketItem> items = basketItemsContext.Collection().ToList();


            if (basket != null)
            {

                var result = (from b in items
                              where b.BasketId == basket.Id
                              join p in
 ProductContext.Collection() on b.ProductId equals p.Id
                              select

                                                new BasketItemViewModel()
                                                {
                                                    Id = b.Id,
                                                    Quantity = b.Quantity,
                                                    ProductName = p.Name,
                                                    Price = p.Price,
                                                    Image = p.Image
                                                }

                                   ).ToList();

                return result;
            }
            else {
                return new List<BasketItemViewModel>();
            }

        }



        public BasketSummaryViewModel GetBasketSummary(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);
            BasketSummaryViewModel model = new BasketSummaryViewModel(0,0);

            ICollection<BasketItem> items = basketItemsContext.Collection().ToList();

            if (basket != null)
            {
                int? basketCount = (from b in items where b.BasketId== basket.Id select b.Quantity).Sum();
                decimal? basketTotal = ( from b in items
                                         where b.BasketId == basket.Id
                                         join p in ProductContext.Collection() on b.ProductId equals p.Id
                                        select b.Quantity*p.Price
                                        ).Sum();

                model.BasketCount = basketCount ?? 0;

                model.BasketTotal = basketTotal ?? decimal.Zero;

                return model;
            }
            else {
                return model;
            }

        }



        public void ClearBasket(HttpContextBase httpContext) {
            Basket basket = GetBasket(httpContext, false);
            basket.BasketItems.Clear();
            BasketContext.Commit();


           // basketItemsContext.Collection().ToList().Clear();
           // basketItemsContext.Commit();

        }



    }
}
 