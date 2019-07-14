using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.ViewModels.Cart;
using WebStore.Domain.ViewModels.Product;
using WebStore.Models;

//Asser как класс из пространства имен Xunit а не из MSTest
using Assert = Xunit.Assert;

namespace WebStore.Services.Tests
{
    [TestClass]
    public class CartServiceTests
    {
        /// <summary>
        /// Проверка что класс корзины возвращает корректное число товаров внутри себя
        /// </summary>
        [TestMethod]
        public void Cart_Class_ItemsCount_Returns_Correct_Quantity()
        {
            //Ожидаемое число товаров в корзине
            const int expected_cout = 5;

            var cart = new Cart {
                Items = new List<CartItem>
                {
                    new CartItem { ProductId = 1, Quantity = 2 },
                    new CartItem { ProductId = 2, Quantity = 1 },
                    new CartItem { ProductId = 3, Quantity = 2 }
                }
            };

            var result = cart.ItemsCount;

            Assert.Equal(expected_cout, result);
        }

        /// <summary>
        /// Проверка вью модели корзины на то что она возвращает корректное число товаров
        /// </summary>
        [TestMethod]
        public void CartViewModel_Returns_Correct_ItemsCount()
        {
            //Ожидаемое число товаров в корзине
            const int expected_cout = 11;

            var cart_view_model = new CartViewModel()
            {
                Items = new Dictionary<ProductViewModel, int>
                {
                    { new ProductViewModel { Id = 1, Name = "Item1", Brand = "", ImageUrl = "", Order = 1, Price = 10m }, 1 },
                    { new ProductViewModel { Id = 2, Name = "Item2", Brand = "", ImageUrl = "", Order = 1, Price = 10m }, 3 },
                    { new ProductViewModel { Id = 3, Name = "Item3", Brand = "", ImageUrl = "", Order = 1, Price = 10m }, 7 }
                }
            };

            var result = cart_view_model.ItemsCount;

            Assert.Equal(expected_cout, result);
        }
    }
}
