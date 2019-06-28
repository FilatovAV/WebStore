using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebStore.Controllers.Implementations;
using WebStore.Controllers.Interfaces;
using WebStore.DAL.Context;
using WebStore.Data;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Implementations;
using WebStore.Infrastructure.Interfaces;
using WebStore.Models;

namespace WebStore
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WebStoreContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<WebStoreContextInitializer>();

            services.AddSingleton<IEmployeesData, InMemoryEmployeesData>();
            services.AddScoped<IProductData, SqlProductData>();
            services.AddScoped<ICartService, CookieCartService>();
            services.AddScoped<IOrderService, SqlOrdersService>();

            //Система идентификации пользователей
            //-----------------------------------------------------------------

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<WebStoreContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(cfg =>
            {
                cfg.Password.RequiredLength = 3;
                cfg.Password.RequireDigit = false;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequiredUniqueChars = 3;

                cfg.Lockout.MaxFailedAccessAttempts = 10;
                cfg.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                cfg.Lockout.AllowedForNewUsers = true;


                cfg.User.RequireUniqueEmail = false; //временно
            });

            //-----------------------------------------------------------------

            //Конфигурирование Cookies
            //-----------------------------------------------------------------

            services.ConfigureApplicationCookie(cfg =>
            {
                cfg.Cookie.HttpOnly = true;
                cfg.Cookie.Expiration = TimeSpan.FromDays(150);
                cfg.Cookie.MaxAge = TimeSpan.FromDays(150);

                cfg.LoginPath = "/Account/Login";
                cfg.LogoutPath = "/Account/Logout";
                cfg.AccessDeniedPath = "/Account/AccessDenied";

                //пользователю который прошел афторизацию будет сменен номер сеанса (для повышения безопасности)
                cfg.SlidingExpiration = true;
            });

            //-----------------------------------------------------------------




            services.AddMvc();

            //подключение фильтров
            //services.AddMvc(f => {
            //    f.Filters.Add<ActionFilter>();
            //});

            //services.AddMvc(opt =>
            //{
            //    //подключение фильтров
            //    //opt.Filters.Add<ActionFilter>();
            //    //подключение соглашений
            //    //opt.Conventions.Add(new Infrastructure.Conventions.TestConvention());
            //});

            services.AddAutoMapper(opt =>
            {
                opt.CreateMap<Employee, Employee>();
            }, typeof(Startup));

            //AutoMapper.Mapper.Initialize(opt =>
            //{
            //    opt.CreateMap<Employee, Employee>();
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, WebStoreContextInitializer db)
        {
            db.InitializeAsync().Wait();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //Соединение с браузером для обновления
                //Microsoft.VisualStudio.Web.BrowserLink
                app.UseBrowserLink();
            }

            //JS/CSS
            app.UseStaticFiles();
            //использовать файлы по умолчанию
            app.UseDefaultFiles();
            //подключение системы аутинтификации
            app.UseAuthentication();

            //Можно задать маршрут по умолчанию
            //app.UseMvcWithDefaultRoute();

            //Данная строчка должна быть заключительной в этом файле, все что после работать не будет
            app.UseMvc( route => {

                route.MapRoute(
                name: "areas",
                template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                route.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");//,
                    //defaults: new
                    //{
                    //    controller = "Home",
                    //    action = "Index",
                    //    id = (int?)null
                    //}
            });

            //ниже ничего не произойдет, UseMVC терминальный вызов
        }
    }
}
