using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using WebStore.Controllers;
using WebStore.Domain.DTO.Order;
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

        //Проверка что контроллекр действительно вызывает метод сервиса
        [TestMethod]
        public void CheckOut_Calls_Service_and_Return_Redirect()
        {
            //Получаем информацию о текущем пользовтаеле
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "1") }));

            //Макетируем сервисы
            var cart_service_mock = new Mock<ICartService>();
            var order_service_mock = new Mock<IOrderService>();

            //-----------------------------------------------------------------------------------------
            //Конфигурируем сервисы

            //Вызов метода TransformCart
            //должен вернуть вью модель корзины CartViewModel
            cart_service_mock.Setup(c => c.TransfomCart()).
                Returns(() => new CartViewModel
                { Items = new Dictionary<ProductViewModel, int>
                {
                    { new ProductViewModel(), 1 }
                }
                });

            //Ожидаемый номер заказа
            const int expected_order_id = 1;

            //Конфигурируем макет сервиса заказов
            //Вызов метода CreateOrder должен вернуть 
            //которому необходимо передать order модель и User.Name и возвращаться объект OrderDTO с идентификатором 1
            order_service_mock.Setup(c => c.CreateOrder(It.IsAny<CreateOrderModel>(), It.IsAny<string>())).
                Returns(new OrderDTO { Id = expected_order_id });

            //-----------------------------------------------------------------------------------------


            //-----------------------------------------------------------------------------------------
            //Создаем контроллер который работает с макетами сервисов
            var controller = new CartController(cart_service_mock.Object, order_service_mock.Object)
            {
                //Изменяем контекст контроллера, он создается в момент создания контроллера
                //Контроллер запоминает идентификатор пользователя который сделал запрос
                //Сделаем это вручную
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = user
                    }
                }
            };

            //Вызываем созданный контроллер
            var result = controller.CheckOut(new OrderViewModel
            {
                Address = "",
                Name = "Test",
                Phone = ""
            });
            //-----------------------------------------------------------------------------------------

            //-----------------------------------------------------------------------------------------
            //Проверка
            var redirect_result = Assert.IsType<RedirectToActionResult>(result);
            //Проверяем что имя контроллера не null
            Assert.Null(redirect_result.ControllerName);
            //Проверяем что перенаправление произошло на метод OrderConfirmed
            Assert.Equal(nameof(CartController.OrderConfirmed), redirect_result.ActionName);
            //Прверяем что в параметрах ответа находится 1 (expected_order_id)(id)
            Assert.Equal(expected_order_id, redirect_result.RouteValues["id"]);
        }
    }
}
