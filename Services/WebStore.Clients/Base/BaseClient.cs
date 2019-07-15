using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;
using System.Threading;

namespace WebStore.Clients.Base
{
    public abstract class BaseClient: IDisposable
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

        //CancellationToken - возможность отмены асинхронной операции
        //Тип данных с которыми работает метод Get имеет конструктор по умолчанию, 
        //чтобы в случае чего мы могли вернуть значения по умолчанию - where T: new()
        protected async Task<T> GetAsync<T>(string url, CancellationToken Cancel = default) where T: new()
        {
            var response = await _Client.GetAsync(url, Cancel);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<T>(Cancel);
            }
            return new T();
        }
        protected T Get<T>(string url) where T: new() => GetAsync<T>(url).Result;

        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T item, CancellationToken Cancel = default)
        {
            return (await _Client.PostAsJsonAsync(url, item, Cancel)).EnsureSuccessStatusCode();
        }
        protected HttpResponseMessage Post<T>(string url, T item) => PostAsync(url, item).Result;

        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T item, CancellationToken Cancel = default)
        {
            return (await _Client.PutAsJsonAsync(url, item, Cancel)).EnsureSuccessStatusCode();
        }
        protected HttpResponseMessage Put<T>(string url, T item) => PutAsync(url, item).Result;

        protected async Task<HttpResponseMessage> DeleteAsync<T>(string url, CancellationToken Cancel = default)
        {
            return (await _Client.DeleteAsync(url, Cancel));
        }
        protected HttpResponseMessage Delete(string url) => _Client.DeleteAsync(url).Result;

        public void Dispose()
        {
            _Client.Dispose();
        }
    }
}
