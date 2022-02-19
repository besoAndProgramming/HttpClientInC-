using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HTTPClientDemo.HttpClientServices
{
    public class CRUDHTTPService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        public CRUDHTTPService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();
        }

        public async Task<List<T>> GetAll<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
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

        public async Task<T> GetById<T>(string url, string id)
        {
            var response = await _httpClient.GetAsync(url + $"/{id}");
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

        public async Task<T> Create<T>(string url, T item)
        {
            var itemJson = new StringContent(
                    JsonConvert.SerializeObject(item),
                    Encoding.UTF8,
                    "application/json");
            var response = await _httpClient.PostAsync(
                url,
                itemJson);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var createdItem = JsonConvert.DeserializeObject<T>(content);

            return createdItem;
        }

        public async Task<T> Update<T>(string url, T item)
        {
            var itemJson = new StringContent(
                   JsonConvert.SerializeObject(item),
                   System.Text.Encoding.UTF8,
                   "application/json");

            var response = await _httpClient.PutAsync(
               url,
              itemJson);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var updatedItem = JsonConvert.DeserializeObject<T>(content);

            return updatedItem;
        }

        public async Task Delete(string url)
        {
            var response = await _httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();

            await response.Content.ReadAsStringAsync();
        }

    }
}
