using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;
using WebStore.Clients.Base;
using WebStore.Controllers.Interfaces;
using WebStore.Models;

namespace WebStore.Clients.Employees
{
    public class EmplyeesClient : BaseClient, IEmployeesData
    {
        public EmplyeesClient(IConfiguration configuration) : base(configuration, "api/employees") { }

        public void AddNew(Employee employee)
        {
            Post(_ServiceAddress, employee);
        }

        public void Delete(int id)
        {
            Delete($"{_ServiceAddress}/{id}");
        }

        public IEnumerable<Employee> GetAll()
        {
            return Get<List<Employee>>(_ServiceAddress);
        }

        public Employee GetById(int id)
        {
            return Get<Employee>($"{_ServiceAddress}/{id}");
        }

        public void SaveChanges()
        {
            //TODO Пока ничего
        }

        public Employee Update(int id, Employee employee)
        {
            var response = Put($"{_ServiceAddress}/{id}", employee);
            return response.Content.ReadAsAsync<Employee>().Result;
        }
    }
}
