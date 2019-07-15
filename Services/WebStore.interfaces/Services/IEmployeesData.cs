using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Controllers.Interfaces
{
    /// <summary>Управление моделью сотрудников</summary>
    public interface IEmployeesData
    {
        /// <summary>Получить всю коллекцию сотрудников</summary>
        IEnumerable<Employee> GetAll();
        /// <summary>Получить сотрудника по Id</summary>
        Employee GetById(int id);
        /// <summary>Добавить сотрудника</summary>
        void AddNew(Employee employee);
        /// <summary> Изменить данные по сотруднику</summary>
        Employee Update(int id, Employee employee);
        /// <summary>Удалить сотрудника</summary>
        //void Delete(Employee employee);
        /// <summary>Удалить сотрудника по Id</summary>
        void Delete(int id);
        /// <summary>Сохранить изменения</summary>
        void SaveChanges();
    }
}
