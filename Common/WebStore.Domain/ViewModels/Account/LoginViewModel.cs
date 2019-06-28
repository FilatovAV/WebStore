using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Domain.ViewModels.Account
{
    public class LoginViewModel
    {
        [Display(Name = "Имя пользователя"), MaxLength(256, ErrorMessage = "Допустимая длина 256 символов")]
        public string UserName { get; set; }
        [Display(Name = "Пароль"), Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }

        /// <summary>Адрес перенаправления пользователя при успешной авторизации</summary>
        public string ReturnUrl { get; set; }
    }
}
