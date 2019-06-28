using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.ViewModels
{
    public class RegisterUserViewModel
    {
        [Display(Name = "Имя пользователя"), MaxLength(256, ErrorMessage = "Допустимая длина 256 символов")]
        public string UserName { get; set; }
        [Display(Name = "Пароль"), Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Подтверждение пароля"), Required, DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
