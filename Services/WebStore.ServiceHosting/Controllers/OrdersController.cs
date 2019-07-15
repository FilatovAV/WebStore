using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.DTO.Order;
using WebStore.Infrastructure.Interfaces;

namespace WebStore.ServiceHosting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase, IOrderService
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService) => this._orderService = orderService;

        [HttpPost("{userName?}")]
        public OrderDTO CreateOrder(CreateOrderModel OrderModel, string UserName)
        {
            return _orderService.CreateOrder(OrderModel, UserName);
        }
        [HttpGet("{id}"), ActionName("Get")]
        public OrderDTO GetOrderById(int id)
        {
            return _orderService.GetOrderById(id);
        }

        [HttpGet("user/{UserName}")]
        public IEnumerable<OrderDTO> GetUserOrders(string UserName)
        {
            return _orderService.GetUserOrders(UserName);
        }
    }
}