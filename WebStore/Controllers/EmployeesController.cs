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

        private static List<Employee> employees = new List<Employee>()
        {
            new Employee
            {
                Id = 0,
                FirstName = "Геннадий",
                SurName = "Воржев",
                Patronymic = "Степанович",
                Age = 45
            },
            new Employee
            {
                Id = 1,
                FirstName = "Алексей",
                SurName = "Рахманин",
                Patronymic = "Викторович",
                Age = 45
            },
            new Employee
            {
                Id = 2,
                FirstName = "Степан",
                SurName = "Войтенков",
                Patronymic = "Денисович",
                Age = 45
            }
        };
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