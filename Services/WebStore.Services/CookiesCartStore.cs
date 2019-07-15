using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebStore.interfaces.Services;
using WebStore.Models;

namespace WebStore.Services
{
    public class CookiesCartStore : ICartStore
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _cartName; //название cookies в которой мы будем хранить данные корзины

        public Cart Cart
        {
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
                }
                else
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


        public CookiesCartStore(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;

            var user = _httpContextAccessor.HttpContext.User;
            var user_name = user.Identity.IsAuthenticated ? user.Identity.Name : String.Empty;

            _cartName = $"cart{user_name}";
        }
    }
}
