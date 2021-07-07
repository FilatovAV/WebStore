using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.interfaces.Api;

namespace WebStore.Controllers
{
    public class WebApiTestController : Controller
    {
        private readonly IValuesService _valuesService;

        public WebApiTestController(IValuesService valuesService)
        {
            this._valuesService = valuesService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _valuesService.GetAsync());
        }
    }
}