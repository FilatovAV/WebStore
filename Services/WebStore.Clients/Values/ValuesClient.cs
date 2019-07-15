using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebStore.Clients.Base;
using WebStore.interfaces.Api;

namespace WebStore.Clients.Values
{
    public class ValuesClient: BaseClient, IValuesService
    {
        public ValuesClient(IConfiguration configuration) : base(
            configuration,
            "api/Values") { }

        public HttpStatusCode Delete(int id)
        {
            return DeleteAsync(id).Result;
        }

        public async Task<HttpStatusCode> DeleteAsync(int id)
        {
            var response = await base._Client.GetAsync($"{_ServiceAddress}/delete/{id}");
            return response.StatusCode;
        }

        public IEnumerable<string> Get()
        {
            return GetAsync().Result;
        }

        public async Task<IEnumerable<string>> GetAsync()
        {
            var response = await base._Client.GetAsync(_ServiceAddress);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<string>>();
            }
            return new string[0];
        }
        public string Get(int id)
        {
            return GetAsync(id).Result;
        }
        public async Task<string> GetAsync(int id)
        {
            var response = await _Client.GetAsync($"{_ServiceAddress}/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<string>();
            }
            return String.Empty;
        }

        public Uri Post(string value)
        {
            return PostAsync(value).Result;
        }

        public async Task<Uri> PostAsync(string value)
        {
            var response = await _Client.PostAsJsonAsync($"{_ServiceAddress}/post", value);
            //Убеждаемся что сервис вернул ответ, в противном случае мы получим ошибку HttpRequestException
            response.EnsureSuccessStatusCode();
            //Если все ок то возвращаем адрес ответа от службы
            return response.Headers.Location;
        }

        public HttpStatusCode Put(int id, string value)
        {
            return PutAsync(id, value).Result;
        }

        public async Task<HttpStatusCode> PutAsync(int id, string value)
        {
            var response = await _Client.PutAsJsonAsync($"{_ServiceAddress}/put/{id}", value);
            //Убеждаемся что сервис вернул ответ, в противном случае мы получим ошибку HttpRequestException
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }
    }
}
