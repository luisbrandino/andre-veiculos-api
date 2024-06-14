using Newtonsoft.Json;
using Models;
using System.Text;

namespace Endpoints
{
    public class HttpClientEndpoint<T> : IEndpoint<T> where T : Model
    {
        private readonly IEndpointSettings _settings;
        private readonly HttpClient _client;

        public HttpClientEndpoint(IEndpointSettings settings)
        {
            _settings = settings;
            _client = new HttpClient();
        }

        public async Task<IEnumerable<T>> Find()
        {
            HttpResponseMessage response = await _client.GetAsync(_settings.Url);

            if (!response.IsSuccessStatusCode)
                return null;

            string data = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<T>>(data);
        }

        public async Task<T?> Find(object id)
        {
            HttpResponseMessage response = await _client.GetAsync(_settings.Url + $"/{id.ToString()}");

            if (!response.IsSuccessStatusCode)
                return null;

            string data = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(data);
        }

        public async Task<T> Insert(T item)
        {
            string json = JsonConvert.SerializeObject(item);

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(_settings.Url, content);

            if (!response.IsSuccessStatusCode)
                return null;

            string data = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
