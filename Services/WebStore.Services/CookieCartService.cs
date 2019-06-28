using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Infrastructure.Interfaces;
using WebStore.Models;
using WebStore.ViewModels;

namespace WebStore.Infrastructure.Implementations
{
    public class CookieCartService : ICartService
    {
        private readonly IProductData _productData;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _cartName; //название cookies в которой мы будем хранить данные карзины

        private Cart Cart {
            get
            {
                var http_context = _httpContextAccessor.HttpContext;
                var cookie = http_context.Request.Cookies[_cartName];

                Cart cart = null;
                if (cookie is null)
                {
                    cart = new Cart();
                    http_context.Response.Cookies.Append(
                        _cartName,
                        JsonConvert.SerializeObject(cart));
                } else
                {
                    cart = JsonConvert.DeserializeObject<Cart>(cookie);
                    http_context.Response.Cookies.Delete(_cartName);
                    http_context.Response.Cookies.Append(_cartName, cookie, new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(1)
                    });
                }
                return cart;
            }
            set
            {
                var http_context = _httpContextAccessor.HttpContext;

                var json = JsonConvert.SerializeObject(value);

                http_context.Response.Cookies.Delete(_cartName);
                http_context.Response.Cookies.Append(_cartName, json, new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(1)
                });
            }
        }

        //IHttpContextAccessor - специальный сервис который предоставляет доступ к контексту запроса
        public CookieCartService(IProductData productData, IHttpContextAccessor httpContextAccessor)
        {
            this._productData = productData;
            this._httpContextAccessor = httpContextAccessor;

            var user = _httpContextAccessor.HttpContext.User;

            _cartName = $"cart{(user.Identity.IsAuthenticated ? user.Identity.Name : String.Empty)}";
        }
        public void AddToCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item != null)
            {
                item.Quantity++;
            } else
            {
                cart.Items.Add(new CartItem() { ProductId = id, Quantity = 1 });
            }

            Cart = cart;
        }

        public void DecrementFromCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item != null)
            {
                if (item.Quantity > 0)
                {
                    item.Quantity--;
                }
                if (item.Quantity == 0)
                {
                    cart.Items.Remove(item);
                }

                Cart = cart;
            }
        }

        public void RemoveAll()
        {
            var cart = Cart;
            cart.Items.Clear();
            Cart = cart;
        }

        public void RemoveFromCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item != null)
            {
                cart.Items.Remove(item);
                Cart = cart;
            }
        }

        public CartViewModel TransfomCart()
        {
            var products = _productData.GetProducts(new Domain.Entities.ProductFilter()
            { ids = Cart.Items.Select(i=>i.ProductId).ToList<int>() });
            var products_view_models = products.Select(product => new ProductViewModel()
            {
                Brand = product.Brand?.Name,
                Id = product.Id,
                ImageUrl = product.ImageUrl,
                Name = product.Name,
                Order = product.Order,
                Price = product.Price
            });
            var cart_view_model = new CartViewModel()
            {
                Items = Cart.Items.ToDictionary(
                    x => products_view_models.First(f => f.Id == x.ProductId), 
                    x => x.Quantity)
            };
            return cart_view_model;
        }
    }
}
