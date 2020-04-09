using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Services
{
    public class OrderService : IOrderService
    {

        IRepository<Order> orderContext;


        public OrderService(IRepository<Order> OrderContext)
        {
            this.orderContext = OrderContext;
        }

        public decimal getTotal(string Id) {
            Order order = GetOrder(Id);

            decimal total = (from p in order.OrderItems select p.Price * p.Quantity).Sum();
            return total;
        }

        public void CreateOrder(Order baseOrder, List<BasketItemViewModel> basketItrems)
        {


            foreach (var item in basketItrems) {

                baseOrder.OrderItems.Add(new OrderItem() { 
                        ProductId = item.Id,
                        Image = item.Image,
                        Price = item.Price,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity
                });

            }

            orderContext.Insert(baseOrder);
            orderContext.Commit();


        }



        public List<Order> GetOrderList() {
            return orderContext.Collection().ToList();
        }


        public Order GetOrder(string Id) {
            return orderContext.Find(Id);
        }


        public void UpdateOrder(Order updateOrder) {
            orderContext.Update(updateOrder);
            orderContext.Commit();
        }


    }
}
