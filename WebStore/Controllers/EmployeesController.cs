using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {

        public IActionResult Index()
        {
            return View(employees);
        }
        public IActionResult Details(int id)
        {
            var employee = employees.Where(i => i.Id == id).FirstOrDefault();

            if (employee == null)
            {
                NotFound();
            }

            return View(employee);

        }
    }
}