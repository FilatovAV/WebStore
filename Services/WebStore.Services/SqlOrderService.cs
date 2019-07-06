using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.DTO.Order;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Interfaces;

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
        public Order CreateOrder(CreateOrderModel OrderModel, string UserName)
        {
            var user = _userManager.FindByNameAsync(UserName).Result;

            using (var trans = _db.Database.BeginTransaction())
            {
                var order = new Order()
                {
                    Name = OrderModel.OrderViewModel.Name,
                    Address = OrderModel.OrderViewModel.Address,
                    Phone = OrderModel.OrderViewModel.Phone,
                    User = user,
                    Date = DateTime.Now
                };

                _db.Orders.Add(order);

                foreach (var item in OrderModel.OrderItems)
                {
                    var product_model = _db.Products.FirstOrDefault(p => p.Id == item.Id);
                    if (product_model is null)
                    {
                        throw new InvalidOperationException($"Product OrderModel.OrderItems.Id {item.Id} in the database is not found!");
                    }
                    var product = _db.Products.FirstOrDefault(p => p.Id == product_model.Id);
                    if (product is null)
                    {
                        throw new InvalidOperationException($"Product ID {product_model.Id} in the database is not found!");
                    }
                    var order_item = new OrderItem()
                    {
                        Order = order,
                        Price = product.Price,
                        Quantity = item.Quantity,
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
