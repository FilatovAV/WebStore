using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels.Cart;
using WebStore.Domain.ViewModels.Product;
using WebStore.Infrastructure.Interfaces;
using WebStore.interfaces.Services;
using WebStore.Models;

namespace WebStore.Infrastructure.Implementations
{
    public class CartService : ICartService
    {
        private readonly IProductData _productData;
        private ICartStore _cartStore;

        //IHttpContextAccessor - специальный сервис который предоставляет доступ к контексту запроса
        public CartService(IProductData productData, ICartStore cartStore)
        {
            this._productData = productData;
            this._cartStore = cartStore;
        }
        public void AddToCart(int id)
        {
            var cart = _cartStore.Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item != null)
            {
                item.Quantity++;
            } else
            {
                cart.Items.Add(new CartItem() { ProductId = id, Quantity = 1 });
            }

            _cartStore.Cart = cart;
        }

        public void DecrementFromCart(int id)
        {
            var cart = _cartStore.Cart;
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

                _cartStore.Cart = cart;
            }
        }

        public void RemoveAll()
        {
            var cart = _cartStore.Cart;
            cart.Items.Clear();
            _cartStore.Cart = cart;
        }

        public void RemoveFromCart(int id)
        {
            var cart = _cartStore.Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item != null)
            {
                cart.Items.Remove(item);
                _cartStore.Cart = cart;
            }
        }

        public CartViewModel TransformCart()
        {
            var products = _productData.GetProducts(new Domain.Entities.ProductFilter()
            { Ids = _cartStore.Cart.Items.Select(i=>i.ProductId).ToList<int>() });
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
                Items = _cartStore.Cart.Items.ToDictionary(
                    x => products_view_models.First(f => f.Id == x.ProductId), 
                    x => x.Quantity)
            };
            return cart_view_model;
        }

        CartViewModel ICartService.TransformCart()
        {
            var products = _productData.GetProducts(new ProductFilter
            {
                Ids = _cartStore.Cart.Items.Select(item => item.ProductId).ToList()
            });

            var products_view_models = products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Order = p.Order,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                Brand = p.Brand?.Name
            });

            return new CartViewModel
            {
                Items = _cartStore.Cart.Items.ToDictionary(
                    x => products_view_models.First(p => p.Id == x.ProductId),
                    x => x.Quantity)
            };
        }
    }
}
