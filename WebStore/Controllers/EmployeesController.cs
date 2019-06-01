using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        public IActionResult Edit(Employee employee, [FromServices] IMapper mapper)
        {
            //Валидация модели данных
            if (employee.Age < 6)
            {
                ModelState.AddModelError("Age", "Возраст должен быть более 6 лет");
            } else if (employee.Age > 120)
            {
                ModelState.AddModelError("Age", "Сомнительный возраст");
            }

            if (employee.Age == 66)
            {
                //Если мы не укажим ключ то ошибка будет привязана к валидации всей формы
                //<div asp-validation-summary="ModelOnly" class="text-danger"></div>
                ModelState.AddModelError("", "Проверка возраста 66");
            }


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

                //AutoMApper DependencyInjection
                mapper.Map(employee, db_employee);

                //обычный AutoMApper
                //AutoMapper.Mapper.Map(employee, db_employee);

                //раньше
                //db_employee.FirstName = employee.FirstName;
                //db_employee.Patronymic = employee.Patronymic;
                //db_employee.SurName = employee.SurName;
                //db_employee.Age = employee.Age;
            } else
            {
                employeesData.AddNew(employee);
                
            }

            employeesData.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}