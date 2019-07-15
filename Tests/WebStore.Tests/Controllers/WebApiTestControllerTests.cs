using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Controllers;
using WebStore.interfaces.Api;

//Asser как класс из пространства имен Xunit а не из MSTest
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class WebApiTestControllerTests
    {
        private WebApiTestController _Controller;

        //Для того чтобы метод стал инициализирующим применяем атрибут
        [TestInitialize]
        public void Initialize()
        {
            //Создаем фейковый элемент IValueService 
            var value_service_mock = new Mock<IValuesService>();

            //Настраиваем фейковый объект, при вызове GetAsync будем получать массив new[] { "qwe", "456", "789" }
            value_service_mock.
                Setup(service => service.GetAsync()).ReturnsAsync(new[] { "123", "456", "789" });

            //value_service_mock.Object - свойство Oject реализует интерфейс IValueObject
            _Controller = new WebApiTestController(value_service_mock.Object);
        }

        [TestMethod]
        public async Task Index_Method_Returns_View_With_Values()
        {
            var result = await _Controller.Index();
            //Получаем представление
            var view_result = Assert.IsType<ViewResult>(result);
            //Извлекаем модель из представления
            var model = Assert.IsAssignableFrom<IEnumerable<string>>(view_result.Model);

            const int expected_count = 3;
            Assert.Equal(expected_count, model.Count());
        }
    }
}
