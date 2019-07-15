using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.DTO.Order;
using WebStore.Domain.Entities;

namespace WebStore.Infrastructure.Interfaces
{
    public interface IOrderService
    {
        IEnumerable<OrderDTO> GetUserOrders(string UserName);
        OrderDTO GetOrderById(int id);
        OrderDTO CreateOrder(CreateOrderModel OrderModel, string UserName);
    }
}
