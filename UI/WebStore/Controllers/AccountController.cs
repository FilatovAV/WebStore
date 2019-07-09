using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels.Account;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            ILogger<AccountController> logger)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._logger = logger;
        }


        /// <summary>Регистрация нового пользователя</summary>
        public IActionResult Register() => View();

        //Позволяет избежать подделки регистрационных данных форм, в паре с asp-antiforgery="true" на представлении
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            //Проверяем на корректность полученной модели данных
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            using (_logger.BeginScope($"Регистрация нового пользователя: {model.UserName}"))
            {

                //Создаем нового пользователя
                var new_user = new User() { UserName = model.UserName };
                //Выполняем попытку зарегистрировать его с указанным паролем
                var creation_result = await _userManager.CreateAsync(new_user, model.Password);

                //В случае успешной регистрации
                if (creation_result.Succeeded)
                {
                    //Входим новым пользователем в систему (без сохранения)
                    await _signInManager.SignInAsync(new_user, false);

                    _logger.LogInformation($"Пользователь {model.UserName} успешно зарегистрирован в системе.");

                    //И переходим на главную страницу сайта
                    return RedirectToAction("Index", "Home");
                }

                //---------------------------------------------------------------------------------
                //Если зарегистрировать пользователя не удалось
                foreach (var item in creation_result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

                string errors_str = String.Join(',', creation_result.Errors.Select(s => s.Description));
                _logger.LogWarning($"Ошибка при регистрации нового пользователя {model.UserName}: {errors_str}");

            }


            return View(model);
        }

        public IActionResult Login() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var login_result = await _signInManager.PasswordSignInAsync(
                login.UserName, login.Password, login.RememberMe, false);

            if (login_result.Succeeded)
            {
                _logger.LogInformation($"Пользователь {login.UserName} вошел в систему");

                if (Url.IsLocalUrl(login.ReturnUrl))
                {
                    return Redirect(login.ReturnUrl);
                } else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Имя пользователя, или пароль неверны!");

            _logger.LogWarning($"Ошибка входа пользователя {login.UserName} в систему");

            return View(login);
        }
        public async Task<IActionResult> LogOut()
        {
            var user_name = User.Identity.Name;
            //Выйти из сеанса и удалить соответствующий cookies
            await _signInManager.SignOutAsync();

            _logger.LogInformation($"Пользователь {user_name} вышел из системы");

            return RedirectToAction("Index", "Home");
        } 
        public IActionResult AccessDenied() => View();
    }
}