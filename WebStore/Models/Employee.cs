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
        //[MinLength(3)] //Ограничение по длине поля
        //Проверим с помощью регулярного выражения на длину данных, сочетание русских и английских букв, заглавную букву в начале
        [RegularExpression(@"(^[А-Я][а-я]{2,150}$)|(^[A-Z][a-z]{2,150}$)", ErrorMessage = "Некорректный формат имени")]
        public string SurName { get; set; }

        [Display(Name = "First name"), Required]
        //[MinLength(2)]
        [RegularExpression(@"(^[А-Я][а-я]{2,150}$)|(^[A-Z][a-z]{2,150}$)", ErrorMessage = "Некорректный формат фамилии")]
        public string FirstName { get; set; }

        [Display(Name = "Patronymic")]
        [RegularExpression(@"(^[А-Я][а-я]{2,150}$)|(^[A-Z][a-z]{2,150}$)", ErrorMessage = "Некорректный формат отчества")]
        public string Patronymic { get; set; }

        [Display(Name = "Age")]
        //[Range(18,130)] ограничение по диапазону
        public int Age { get; set; }
    }
}
