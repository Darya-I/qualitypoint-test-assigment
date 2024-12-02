namespace AddressService.Services
{
    /// <summary>
    /// модель конфигурации для DaData API
    /// </summary>
    
    public class DaDataSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
    }
}
