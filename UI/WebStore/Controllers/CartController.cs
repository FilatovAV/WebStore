using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.DTO.Order;
using WebStore.Infrastructure.Interfaces;
using WebStore.Domain.ViewModels.Order;
using WebStore.Domain.ViewModels.Cart;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;

        public CartController(ICartService cartService, IOrderService orderService)
        {
            this._cartService = cartService;
            this._orderService = orderService;
        }
        public IActionResult Details()
        {
            var model = new DetailsViewModel
            {
                CartViewModel = _cartService.TransfomCart(),
                OrderViewModel = new OrderViewModel()
            };

            return View(model);
        }

        public IActionResult DecrementFromCart(int id)
        {
            _cartService.DecrementFromCart(id);
            return RedirectToAction("Details");
        }

        public IActionResult RemoveFromCart(int id)
        {
            _cartService.RemoveFromCart(id);
            return RedirectToAction("Details");
        }

        public IActionResult RemoveAll()
        {
            _cartService.RemoveAll();
            return RedirectToAction("Details");
        }

        public IActionResult AddToCart(int id)
        {
            _cartService.AddToCart(id);
            return RedirectToAction("Details");
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CheckOut(OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Details), new DetailsViewModel()
                {
                    CartViewModel = _cartService.TransfomCart(),
                    OrderViewModel = model
                });
            }

            var create_model = new CreateOrderModel
            {
                OrderViewModel = model,
                OrderItems = _cartService.TransfomCart().Items.Select(i => new OrderItemDTO
                {
                    Id = i.Key.Id,
                    Price = i.Key.Price,
                    Quantity = i.Value
                }).ToList()
            };

            var order = _orderService.CreateOrder(create_model, User.Identity.Name);

            _cartService.RemoveAll();

            return RedirectToAction("OrderConfirmed", new { id = order.Id });
        }

        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}