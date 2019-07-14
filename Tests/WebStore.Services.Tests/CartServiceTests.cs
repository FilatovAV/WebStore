using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.ViewModels.Cart;
using WebStore.Domain.ViewModels.Product;
using WebStore.Infrastructure.Implementations;
using WebStore.Infrastructure.Interfaces;
using WebStore.interfaces.Services;
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

        /// <summary>
        /// Тест. Добавление данных в корзину.
        /// </summary>
        [TestMethod]
        public void CartService_AddToCart_WorkCorrect()
        {
            //Создаем пустую корзину
            var cart = new Cart
            {
                Items = new List<CartItem>()
            };

            //Создаем макет сервиса ProductData
            var product_data_mok = new Mock<IProductData>();
            //Создаем макет сервиса хранилища данных корзины
            var cart_store_mock = new Mock<ICartStore>();
            //Конфигурируем хранилище корзины
            cart_store_mock
                .Setup(c => c.Cart) //Обращение к свойству Cart
                .Returns(cart);     //будет возвращать корзину определенную выше
            //Создаем сервис корзины
            var cart_service = new CartService(product_data_mok.Object, cart_store_mock.Object);
            //Ожидаемый id
            const int expected_id = 5;
            //Обращаемся к сервису и просим его добавить в корзину товар с идентификатором expected_id
            cart_service.AddToCart(expected_id);

            //-------------------------------------------------------------------------------------------------
            //Проверка

            //В коризне находится 1 товар?
            Assert.Equal(1, cart.ItemsCount);
            //Кол-во записей в корзине 1?
            Assert.Single(cart.Items);
            //product_id у нас ожидаемый?
            Assert.Equal(expected_id, cart.Items[0].ProductId);

        }
        /// <summary>
        /// Тест. Удаление из корзины.
        /// </summary>
        [TestMethod]
        public void CartService_RemoveFromCart_Remove_Correct_Item()
        {
            const int item_id = 1;
            //Создаем пустую корзину
            var cart = new Cart
            {
                //И положим в нее несколько товаров
                Items = new List<CartItem>
                {
                    //Добавим запись с идентификатором который будем удалять
                    new CartItem{ ProductId = item_id, Quantity = 1},
                    //Добавим товар который должен остаться в корзине с отличающимся id и кол-вом
                    new CartItem{ ProductId = 2, Quantity = 3}
                }
            };

            //Создаем макет сервиса ProductData
            var product_data_mok = new Mock<IProductData>();
            //Создаем макет сервиса хранилища данных корзины
            var cart_store_mock = new Mock<ICartStore>();
            //Конфигурируем хранилище корзины
            cart_store_mock
                .Setup(c => c.Cart) //Обращение к свойству Cart
                .Returns(cart);     //будет возвращать корзину определенную выше
            //Создаем сервис корзины
            var cart_service = new CartService(product_data_mok.Object, cart_store_mock.Object);
            //Вызываем метод удаления из корзины
            cart_service.RemoveFromCart(item_id);

            //Проверка
            //Проверяем что один из товаров был удален и остался тот что ожидалось
            Assert.Equal(1, cart.Items.Count);
            Assert.Equal(2, cart.Items[0].ProductId);

        }
    }
}
