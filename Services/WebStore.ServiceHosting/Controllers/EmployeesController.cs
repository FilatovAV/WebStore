using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Controllers.Interfaces;
using WebStore.Models;

namespace WebStore.ServiceHosting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")] //Мы хотим чтобы контроллер работал с данными в формате json
    public class EmployeesController : ControllerBase, IEmployeesData
    {
        private readonly IEmployeesData _emplyeesData;
        public EmployeesController(IEmployeesData emplyeesData) => this._emplyeesData = emplyeesData;

        [HttpPost, ActionName("Post")]
        public void AddNew([FromBody]Employee employee)
        {
            _emplyeesData.AddNew(employee);
        }
        [HttpDelete]
        public void Delete([FromBody]Employee employee)
        {
            _emplyeesData.Delete(employee);
        }
        //Выполнять логирование можно следующим образом
        //[HttpDelete("{id}")]
        //public void Delete(int id, [FromServices]ILogger<EmployeesController> logger)
        //{
        //    using (logger.BeginScope("Delete data"))
        //    {
        //        _emplyeesData.Delete(id);
        //        logger.LogInformation("Data deleted.");
        //    }
        //}
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _emplyeesData.Delete(id);
        }
        [HttpGet, ActionName("Get")]
        public IEnumerable<Employee> GetAll()
        {
            return _emplyeesData.GetAll();
        }
        [HttpGet("{id}"), ActionName("Get")]
        public Employee GetById(int id)
        {
            return _emplyeesData.GetById(id);
        }
        [NonAction]
        public void SaveChanges()
        {
            _emplyeesData.SaveChanges();
        }
        [HttpPut("{id}"), ActionName("Put")]
        public Employee Update(int id, [FromBody]Employee employee)
        {
            return _emplyeesData.Update(id, employee);
        }
    }
}