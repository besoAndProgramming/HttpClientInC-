using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace HTTPClientDemo.HttpClientServices
{
    public class NamedCRUDHTTPService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        private readonly Uri _url;
        public NamedCRUDHTTPService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("Posts");
            _url = _httpClient.BaseAddress;
        }

        public async Task<List<T>> GetAll<T>()
        {
            var response = await _httpClient.GetAsync(_url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = new List<T>();
            if (response.Content.Headers.ContentType.MediaType == "application/json")
            {
                result = JsonConvert.DeserializeObject<List<T>>(content);
            }
            else if (response.Content.Headers.ContentType.MediaType == "application/xml")
            {
                var serializer = new XmlSerializer(typeof(List<T>));
                result = (List<T>)serializer.Deserialize(new StringReader(content));
            }

            return result;
        }

        public async Task<T> GetById<T>(string id)
        {
            var response = await _httpClient.GetAsync(_url + $"/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = (T)Activator.CreateInstance(typeof(T));
            if (response.Content.Headers.ContentType.MediaType == "application/json")
            {
                result = JsonConvert.DeserializeObject<T>(content);
            }
            else if (response.Content.Headers.ContentType.MediaType == "application/xml")
            {
                var serializer = new XmlSerializer(typeof(T));
                result = (T)serializer.Deserialize(new StringReader(content));
            }

            return result;
        }

        public async Task<T> Create<T>(T item)
        {
            var itemJson = new StringContent(
                    JsonConvert.SerializeObject(item),
                    Encoding.UTF8,
                    "application/json");
            var response = await _httpClient.PostAsync(
                _url,
                itemJson);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var createdItem = JsonConvert.DeserializeObject<T>(content);

            return createdItem;
        }

        public async Task<T> Update<T>(T item)
        {
            var itemJson = new StringContent(
                   JsonConvert.SerializeObject(item),
                   System.Text.Encoding.UTF8,
                   "application/json");

            var response = await _httpClient.PutAsync(
               _url,
              itemJson);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var updatedItem = JsonConvert.DeserializeObject<T>(content);

            return updatedItem;
        }

        public async Task Delete()
        {
            var response = await _httpClient.DeleteAsync(_url);
            response.EnsureSuccessStatusCode();

            await response.Content.ReadAsStringAsync();
        }
    }
}
