using Dadata;
using Dadata.Model;
using Microsoft.Extensions.Options;
using System.Security.Cryptography.Xml;

namespace AddressService.Services
{

    /// <summary>
    /// сервис для работы с api
    /// </summary>
    
    public interface IDadataService
    {
        Task<Address?> StandardizeAddressAsync(string input);
    }


    public class DaDataService : IDadataService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<DaDataService> _logger;
        private readonly DaDataSettings _options;

        public DaDataService(
            IHttpClientFactory httpClientFactory,
            IOptions<DaDataSettings> options,
            ILogger<DaDataService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _options = options.Value;

            _logger.LogInformation("Dadata service init with base URL {BaseUrl}", _options.BaseUrl);
        }

        public async Task<Address?> StandardizeAddressAsync(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                _logger.LogWarning("Empty or null address provided for standardization.");
                throw new ArgumentException("Address cannot be empty", nameof(address));
            }

            try
            {
                _logger.LogInformation("Standardizing address: {Address}", address);

                //получить http клиент
                var httpClient = _httpClientFactory.CreateClient("DadataClient");

                //api-клиент дадата
                var apiClient = new CleanClientAsync(_options.ApiKey, _options.Secret, _options.BaseUrl, httpClient);

                //запрос
                var result = await apiClient.Clean<Address>(address);

                if (result == null)
                {
                    _logger.LogWarning("Dadata API returned null for: {Address}", address);
                    throw new ApplicationException("Failed to standardize address");
                }
                _logger.LogInformation("Success");
                return result;
            }

            catch (Exception ex)
            {
                // логируем перед передачей в middleware
                _logger.LogError(ex, "Error while standardizing address: {Address}", address);
                throw;
            }
        }

    }
}
