using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Controllers.Interfaces;
using WebStore.Models;

namespace WebStore.Controllers.Implementations
{
    public class InMemoryEmployeesData : IEmployeesData
    {
        private readonly List<Employee> employees = new List<Employee>()
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

        public void AddNew(Employee employee)
        {
            if (employee is null) { throw new ArgumentException(nameof(employee)); }
            if (!employees.Contains(employee) || !employees.Any(i=> i.Id == employee.Id))
            {
                employees.Add(employee);
            }
        }

        public void Delete(Employee employee)
        {
            if (employee is null) { throw new ArgumentException(nameof(employee)); }
            if (employees.Contains(employee) || employees.Any(i => i.Id == employee.Id))
            {
                employees.Remove(employee);
            }
        }

        public void Delete(int id)
        {
            var employee = GetById(id);
            if (employee is null) { return; }
            employees.Remove(employee);
        }

        public IEnumerable<Employee> GetAll() => employees;

        public Employee GetById(int id) => employees.FirstOrDefault(f => f.Id == id);

        public void SaveChanges()
        {
            //throw new NotImplementedException();
        }
    }
}
