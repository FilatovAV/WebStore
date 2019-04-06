using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Controllers.Interfaces
{
    public interface IEmployeesData
    {
        IEnumerable<Employee> GetAll();
        Employee GetById(int id);
        void AddNew(Employee employee);
        void Delete(Employee employee);
        void Delete(int id);
        void SaveChanges();
    }
}
