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
        private readonly HttpClient _Client;
        protected readonly string _ServiceAddress;
        BaseClient(IConfiguration configuration, string serviceAddress)
        {
            _ServiceAddress = serviceAddress;

            _Client = new HttpClient { BaseAddress = new Uri(configuration["ClientAddress"]) };
            _Client.DefaultRequestHeaders.Accept.Clear();
            //Указываем клиенту что бы он отправлял сервису заголовок что он ожидает обратно данные в формате json
            _Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }
    }
}
