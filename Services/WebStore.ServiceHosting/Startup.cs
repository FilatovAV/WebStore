using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using WebStore.Controllers.Implementations;
using WebStore.Controllers.Interfaces;
using WebStore.DAL.Context;
using WebStore.Data;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Implementations;
using WebStore.Infrastructure.Interfaces;
//-------------------------------------
//Для использования log4net
using Microsoft.Extensions.Logging;
using WebStore.Logger;
//-------------------------------------

namespace WebStore.ServiceHosting
{
    public class Startup
    {
        //Получаем объект конфигурации сервиса в конструкторе сервиса
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        //Формирование контенера сервисов
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WebStoreContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<WebStoreContextInitializer>();

            services.AddIdentity<User, IdentityRole>().
                AddEntityFrameworkStores<WebStoreContext>().
                AddDefaultTokenProviders();

            services.AddSingleton<IEmployeesData, InMemoryEmployeesData>();

            services.AddScoped<IProductData, SqlProductData>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderService, SqlOrdersService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ICartService, CartService>();

            //Конфигурироем ПО для документации
            services.AddSwaggerGen(opt => 
                {
                    opt.SwaggerDoc( "Ver1.0", new Info { Title = "WebStore.API", Version = "v1.0" });
                    var xml_file = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xml_path = System.IO.Path.Combine(AppContext.BaseDirectory, xml_file);
                    opt.IncludeXmlComments(xml_path);
                });
        }

        //Прочие конфигурации приложения
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            WebStoreContextInitializer db, ILoggerFactory log)
        {
            log.AddLog4Net();

            db.InitializeAsync().Wait();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //Страница ошибок базы данных
                app.UseDatabaseErrorPage();
            }

            //Конфигурация swagger
            app.UseSwagger();
            app.UseSwaggerUI( opt=> 
            {
                opt.SwaggerEndpoint("swagger/Ver1.0/swagger.json", "WebStore.API");
                opt.RoutePrefix = String.Empty;
            });
            //------------------------------------------------------------------


            app.UseMvc();
        }
    }
}
