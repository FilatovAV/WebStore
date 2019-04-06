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
            return View(employeesData.GetAll());
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
        public IActionResult Edit(int? id)
        {
            Employee employee;
            if (id != null)
            {
                employee = employeesData.GetById((int)id);
                if (employee == null)
                {
                    return NotFound();
                }
            }
            else
            {
                employee = new Employee();
            }

            return View(employee);
        }

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return View(employee);
            }
            else if(employee.Id > 0)
            {
                var db_employee = employeesData.GetById(employee.Id);
                if (db_employee is null)
                {
                    return NotFound();
                }
                db_employee.FirstName = employee.FirstName;
                db_employee.Name = employee.Name;
                db_employee.Patronymic = employee.Patronymic;
                db_employee.SurName = employee.Patronymic;
                db_employee.Age = employee.Age;
            } else
            {
                employeesData.AddNew(employee);
                
            }

            employeesData.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}