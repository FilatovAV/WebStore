using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;

namespace WebStore.Clients.Base
{
    public abstract class BaseClient
    {
        protected readonly HttpClient _Client;
        protected readonly string _ServiceAddress;
        public BaseClient(IConfiguration configuration, string serviceAddress)
        {
            _ServiceAddress = serviceAddress;

            //Должно полностью совпадать с appsettings.json в главном проекте (WebStore/appsettings.json)
            //"ClientAddress": "http://localhost:5001", адрес указывается в WebStore.ServiceHosting/Debug/App URL
            _Client = new HttpClient { BaseAddress = new Uri(configuration["ClientAddress"]) };
            _Client.DefaultRequestHeaders.Accept.Clear();
            //Указываем клиенту что бы он отправлял сервису заголовок что он ожидает обратно данные в формате json
            _Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        protected BaseClient()
        {
        }
    }
}
