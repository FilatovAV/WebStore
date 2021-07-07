using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;

namespace WebStore.Data
{
    public class WebStoreContextInitializer
    {
        private readonly WebStoreContext _db;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public WebStoreContextInitializer(WebStoreContext db, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this._db = db;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public async Task InitializeAsync()
        {
            await _db.Database.MigrateAsync();

            //Инициализация пользователей
            await InitializeIdentityAsync();

            //Если база данных не пустая значит считаем ее инициализированной и выходим
            if (await _db.Products.AnyAsync())
            {
                return;
            }

            using (var transaction = _db.Database.BeginTransaction())
            {
                await _db.Sections.AddRangeAsync(TestData.Sections);

                //Допускаем вставку идентификаторов столбца в таблицу, затем отключаем этот параметр
                await _db.Database.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Sections] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Sections] OFF");

                transaction.Commit();
            }

            using (var transaction = _db.Database.BeginTransaction())
            {
                await _db.Brands.AddRangeAsync(TestData.Brands);

                //Допускаем вставку идентификаторов столбца в таблицу, затем отключаем этот параметр
                await _db.Database.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Brands] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Brands] OFF");

                transaction.Commit();
            }

            using (var transaction = _db.Database.BeginTransaction())
            {
                await _db.Products.AddRangeAsync(TestData.Products);

                //Допускаем вставку идентификаторов столбца в таблицу, затем отключаем этот параметр
                await _db.Database.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Products] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Products] OFF");

                transaction.Commit();
            }
        }

        private async Task InitializeIdentityAsync()
        {
            if (!await _roleManager.RoleExistsAsync(User.RoleUser))
            {
                await _roleManager.CreateAsync(new IdentityRole(User.RoleUser) );
            }
            if (!await _roleManager.RoleExistsAsync(User.RoleAdmin))
            {
                await _roleManager.CreateAsync(new IdentityRole(User.RoleAdmin));
            }

            if (await _userManager.FindByNameAsync(User.AdminUserName) == null)
            {
                var admin = new User
                {
                    UserName = User.AdminUserName,
                    Email = $"{User.AdminUserName}@server.ru"
                };

                var creation_result = await _userManager.CreateAsync(admin, User.DefaultAdminPassword);

                if(creation_result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, User.RoleAdmin);
                }
            }
        }
    }
}
