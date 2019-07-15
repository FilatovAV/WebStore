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
using WebStore.Domain.ViewModels.Product;
using WebStore.Infrastructure.Interfaces;

//Asser как класс из пространства имен Xunit а не из MSTest
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class CatalogControllerTests
    {
        private CatalogController _Controller;

        [TestMethod]
        public void ProductDetails_Retuns_View_With_Correct_Item()
        {
            //A A A
            //Arrange Act Assert

            //Подготовка данных для тестирования и данных для проверки
            #region Arrange

            //Ожидаемый идентификатор
            const int expected_id = 1;
            //Ожидаемое имя
            var expected_name = $"Item id {expected_id}";
            //Ожидаемая цена
            var expected_price = 10m;
            //Ожидаемое имя бренда
            var expected_brand_name = $"Brand of id {expected_id}";


            //Создаем фейковый элемент IProductData (макет интерфейса)
            var product_data_mock = new Mock<IProductData>();
            //Настраиваем фейковый объект, для этого передаем в метод GetProductById некое число It.IsAny<int>()
            product_data_mock.
                Setup(service => service.GetProductById(It.IsAny<int>())).
                Returns<int>(id => new ProductDTO
                {
                    Id = id,
                    Name = $"Item id {id}",
                    ImageUrl = $"Image_id_{id}.png",
                    Order = 0,
                    Price = expected_price,
                    Brand = new BrandDTO { Id = 1, Name = $"Brand of id {id}" }
                });

            //Передаем подготовленный объект контроллеру
            _Controller = new CatalogController(product_data_mock.Object);

            #endregion

            //Процесс вызова тестируемого кода
            #region Act

            var result = _Controller.ProductDetails(expected_id);

            #endregion

            //Проверка полученных результатов
            #region Assert

            //Проверяем что в результате мы получили представление
            var view_result = Assert.IsType<ViewResult>(result);
            //Извлекаем модель из представления
            var model = Assert.IsAssignableFrom<ProductViewModel>(view_result.ViewData.Model);
            //Выполняем проверку полученной модели
            Assert.Equal(expected_id, model.Id);
            Assert.Equal(expected_name, model.Name);
            Assert.Equal(expected_price, model.Price);
            Assert.Equal(expected_brand_name, model.Brand);

            #endregion
        }

        //Если тест будет выполняться больше чем 700 миллисекунд Timeout(700) то тест будет считаться проваленным
        [TestMethod, Description("Описание модульного теста"), Timeout(700), Priority(1)]
        public void ProductDetails_Returns_NotFound()
        {
            //Создаем фейковый элемент IProductData (макет интерфейса)
            var product_data_mock = new Mock<IProductData>();
            //Настраиваем фейковый объект, для этого передаем в метод GetProductById некое число It.IsAny<int>()
            product_data_mock.
                Setup(service => service.GetProductById(It.IsAny<int>())).
                Returns(default(ProductDTO));
            //Передаем подготовленный объект контроллеру
            _Controller = new CatalogController(product_data_mock.Object);

            //Процесс вызова тестируемого кода
            #region Act

            var result = _Controller.ProductDetails(1);

            #endregion

            //Проверка полученных результатов
            #region Assert

            //Проверяем что в результате мы получили NotFoundResult
            Assert.IsType<NotFoundResult>(result);

            #endregion
        }
        [TestMethod]
        public void Shop_Returns_Correct_View()
        {
            //Подготовка данных для тестирования и данных для проверки
            #region Arrange

            //Создаем фейковый элемент IProductData (макет интерфейса)
            var product_data_mock = new Mock<IProductData>();
            //Настраиваем фейковый объект, для этого передаем в метод GetProductById некое число It.IsAny<int>()
            product_data_mock.
                Setup(service => service.GetProducts(It.IsAny<ProductFilter>())).
                Returns<ProductFilter>(filter => new[] 
                {
                    new ProductDTO()
                    {
                        Id = 1,
                        Name = "Product 1",
                        Order = 0,
                        ImageUrl = "Product1.png",
                        Price = 10m,
                        Brand = new BrandDTO
                        {
                            Id = 1,
                            Name = "Brand of Product 1"
                        }
                    },
                    new ProductDTO()
                    {
                        Id = 2,
                        Name = "Product 2",
                        Order = 0,
                        ImageUrl = "Product2.png",
                        Price = 10m,
                        Brand = new BrandDTO
                        {
                            Id = 1,
                            Name = "Brand of Product 2"
                        }
                    },
                    new ProductDTO()
                    {
                        Id = 3,
                        Name = "Product 3",
                        Order = 0,
                        ImageUrl = "Product3.png",
                        Price = 10m,
                        Brand = new BrandDTO
                        {
                            Id = 1,
                            Name = "Brand of Product 3"
                        }
                    }
                });
            //Передаем подготовленный объект контроллеру
            _Controller = new CatalogController(product_data_mock.Object);

            //Ожидаемые значения
            const int expected_section_id = 1;
            const int expected_brand_id = 5;

            #endregion


            //Процесс вызова тестируемого кода
            #region Act
            var result = _Controller.Shop(expected_section_id, expected_brand_id);

            #endregion

            //Проверка полученных результатов
            #region Assert
            //Проверяем что в результате мы получили представление
            var view_result = Assert.IsType<ViewResult>(result);
            //Извлекаем модель из представления
            var model = Assert.IsAssignableFrom<CatalogViewModel>(view_result.ViewData.Model);

            Assert.Equal(3, model.Products.Count());
            Assert.Equal(expected_brand_id, model.BrandId);
            Assert.Equal(expected_section_id, model.SectionId);

            Assert.Equal("Brand of Product 1", model.Products.First().Brand);
            #endregion
        }
    }
}
