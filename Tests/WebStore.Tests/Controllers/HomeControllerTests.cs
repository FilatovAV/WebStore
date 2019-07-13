﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Controllers;

//Asser как класс из пространства имен Xunit а не из MSTest
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {

        private HomeController _Controller;

        [TestInitialize]
        public void Initialize()
        {
            _Controller = new HomeController();
        }
        [TestMethod]
        public void Index_Returns_View()
        {
            var result = _Controller.Index();
            Assert.IsType<ViewResult>(result);
        }
        [TestMethod]
        public void Error404_Returns_View()
        {
            var result = _Controller.Error404();
            Assert.IsType<ViewResult>(result);
        }
        [TestMethod]
        public void Blog_Returns_View()
        {
            var result = _Controller.Blog();
            Assert.IsType<ViewResult>(result);
        }
        [TestMethod]
        public void BlogSingle_Returns_View()
        {
            var result = _Controller.BlogSingle();
            Assert.IsType<ViewResult>(result);
        }
        [TestMethod]
        public void ContactUs_Returns_View()
        {
            var result = _Controller.ContactUs();
            Assert.IsType<ViewResult>(result);
        }

    }
}
