using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebStore.Controllers;
using WebStore.Domain.DTO.Product;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels.Cart;
using WebStore.Domain.ViewModels.Order;
using WebStore.Domain.ViewModels.Product;
using WebStore.Infrastructure.Interfaces;

//Asser как класс из пространства имен Xunit а не из MSTest
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    //Тестирование методов в которых происходит валидация модели
    [TestClass]
    public class CartControllerTests
    {
        [TestMethod]
        public void Checkout_ModelState_Invalid_Returns_ViewModel()
        {
            var cart_service_mock = new Mock<ICartService>();
            var order_service_mock = new Mock<IOrderService>();
            var controller = new CartController(cart_service_mock.Object, order_service_mock.Object);

            controller.ModelState.AddModelError("TestError", "Invalid Model on unit testing");

            const string expected_model_name = "Test order";

            var result = controller.CheckOut(new OrderViewModel { Name = expected_model_name });

            var view_result = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<DetailsViewModel>(view_result.ViewData.Model);
            Assert.Equal(expected_model_name, model.OrderViewModel.Name);

        }
    }
}
