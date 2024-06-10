using Models;
using System.Runtime.ConstrainedExecution;
using System.Text.Json;

namespace AndreVeiculos.Address.PostalCodeServices
{
    public class ViaCepAddressResult : IAddressResult
    {
        public string Street { get; set; }
        public string StreetType { get; set; }
        public string PostalCode { get; set; }
        public string Neighborhood { get; set; }
        public string State { get; set; }
        public string City { get; set; }

        public static ViaCepAddressResult FromJson(string json)
        {
            var root = JsonDocument.Parse(json).RootElement;

            return new ViaCepAddressResult
            {
                Street = root.GetProperty("logradouro").GetString(),
                StreetType = root.GetProperty("logradouro").GetString().Split(" ").First(), 
                PostalCode = root.GetProperty("cep").GetString(),
                Neighborhood = root.GetProperty("bairro").GetString(),
                State = root.GetProperty("uf").GetString(),
                City = root.GetProperty("localidade").GetString()
            };
        }
    }

    public class ViaCepService : IPostalCodeService
    {
        private string _url = "https://viacep.com.br/ws/";

        public async Task<IAddressResult?> Fetch(string postalCode)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(_url + postalCode + "/json");

                if (!response.IsSuccessStatusCode)
                    return null;

                string data = await response.Content.ReadAsStringAsync();

                return ViaCepAddressResult.FromJson(data);
            }
        }
    }
}
