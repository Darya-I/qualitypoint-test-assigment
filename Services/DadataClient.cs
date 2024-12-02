using AddressService.Models;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace AddressService.Services
{
      
    public interface IDadataClient
    {
        Task<string> CleanAddressAsync(string address);
    }

    public class DadataClient
    {

        private readonly DaDataSettings _settings;
        private readonly HttpClient _httpClient;
        private readonly ILogger<DadataClient> _logger;

        public DadataClient(HttpClient httpClient, IOptions<DaDataSettings> settings, ILogger<DadataClient> logger) 
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _logger = logger;

            _httpClient.BaseAddress = new Uri(_settings.BaseUrl);

            _logger.LogInformation("Dadata client init with base URL {BaseUrl}", _settings.BaseUrl);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("Authorization", _settings.ApiKey);
            _httpClient.DefaultRequestHeaders.Add("X-Secret", _settings.Secret);

        }

        public async Task<CleanAddress[]> CleanAddressAsync(string address)
        {
            var requestData = new[] { address };

            var content = new StringContent(
                JsonSerializer.Serialize(requestData), // массив строк
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("clean/address", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("DaData API error. Status code: {StatusCode}, Response: {Response}", response.StatusCode, errorContent);
                return null;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Received response from DaData API: {ResponseContent}", responseContent);

            return JsonSerializer.Deserialize<CleanAddress[]>(responseContent);
        }



    }
}
