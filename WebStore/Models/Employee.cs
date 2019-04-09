using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    //За отображение валидации всей формы, отвечает следующая строка
    //<div asp-validation-summary="ModelOnly" class="text-danger"></div>
    //За отображение валидации для отдельных полей представления, отвечает следующая строка
    //<span asp-validation-for="SurName" class="text-danger"></span>

    public class Employee
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        //Проверка на наличие данных и указываем какое сообщение должно быть выведено на экран
        [Display(Name = "Surname"), Required(ErrorMessage = "Поле является обязательным для заполнения")] 
        [MinLength(3)] //Ограничение по длине поля
        public string SurName { get; set; }

        [Display(Name = "First name"), Required]
        [MinLength(2)]
        public string FirstName { get; set; }

        [Display(Name = "Patronymic")]
        public string Patronymic { get; set; }

        [Display(Name = "Age")]
        public int Age { get; set; }
    }
}
