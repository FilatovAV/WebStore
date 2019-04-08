﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebStore.Controllers.Implementations;
using WebStore.Controllers.Interfaces;
using WebStore.Infrastructure.Implementations;
using WebStore.Infrastructure.Interfaces;

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
            services.AddSingleton<IEmployeesData, InMemoryEmployeesData>();
            services.AddSingleton<IProductData, InMemoryProductsData>();

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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            //Можно задать маршрут по умолчанию
            //app.UseMvcWithDefaultRoute();

            //Данная строчка должна быть заключительной в этом файле, все что после работать не будет
            app.UseMvc( route => {
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
