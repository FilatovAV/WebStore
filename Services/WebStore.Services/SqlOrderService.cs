using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Infrastructure.Implementations
{
    public class SqlOrdersService : IOrderService
    {
        private readonly WebStoreContext _db;
        private readonly UserManager<User> _userManager;

        public SqlOrdersService(WebStoreContext db, UserManager<User> userManager)
        {
            this._db = db;
            this._userManager = userManager;
        }
        public Order CreateOrder(OrderViewModel OrderModel, CartViewModel CartModel, string UserName)
        {
            var user = _userManager.FindByNameAsync(UserName).Result;

            using (var trans = _db.Database.BeginTransaction())
            {
                var order = new Order()
                {
                    Name = OrderModel.Name,
                    Address = OrderModel.Address,
                    Phone = OrderModel.Phone,
                    User = user,
                    Date = DateTime.Now
                };

                _db.Orders.Add(order);

                foreach (var item in CartModel.Items)
                {
                    var product_model = item.Key;
                    var quantity = item.Value;
                    var product = _db.Products.FirstOrDefault(p => p.Id == product_model.Id);
                    if (product is null)
                    {
                        throw new InvalidOperationException($"Product ID {product_model.Id} in the database is not found!");
                    }
                    var order_item = new OrderItem()
                    {
                        Order = order,
                        Price = product.Price,
                        Quantity = quantity,
                        Product = product
                    };

                    _db.OrderItems.Add(order_item);
                }
                _db.SaveChanges();
                trans.Commit();

                return order;
            }
        }

        public Order GetOrderById(int id)
        {
            return _db.Orders.Include(o => o.OrderItems).FirstOrDefault(o => o.Id == id);
        }

        public IEnumerable<Order> GetUserOrders(string userName)
        {
            return _db.Orders
                .Include(i => i.User)
                .Include(i => i.OrderItems)
                .Where(i => i.User.UserName == userName)
                .ToArray();
        }
    }
}
