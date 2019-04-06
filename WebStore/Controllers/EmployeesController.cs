using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Controllers.Interfaces;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData employeesData;

        public EmployeesController(IEmployeesData employeesData)
        {
            this.employeesData = employeesData;
        }
        public IActionResult Index()
        {
            return View(employeesData);
        }
        public IActionResult Details(int id)
        {
            var employee = employeesData.GetById(id);

            if (employee == null)
            {
                NotFound();
            }

            return View(employee);

        }
    }
}