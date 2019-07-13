using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        //[ActionFilterAsync]
        //public IActionResult Index() => View();
        public IActionResult Index()
        {
            //Тестирование логирования ошибок входящих запросов
            //throw new ApplicationException("Тестовое исключение");
            return View();
        }

        public IActionResult Error404() => View();
        public IActionResult Blog() => View();
        public IActionResult BlogSingle() => View();
        //public IActionResult Cart() => View();
        //public IActionResult Checkout() => View();
        public IActionResult ContactUs() => View();
        //public IActionResult ProductDetails() => View();
        public IActionResult ErrorStatusCode(string id)
        {
            switch (id)
            {
                case "404":
                    return RedirectToAction(nameof(Error404));

                default:
                    return Content($"Ошибка в коде {id}");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}