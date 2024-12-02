namespace AddressService.Models
{
    /// <summary>
    /// модель для маппинга ответа от DaData и отправки клиенту
    /// поля, которые могут быть опциональными, остаются nullable:
    /// </summary>

    public class AddressResponce
    {
        public string Country { get; set; }
        public string? Region { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? HouseNumber { get; set; }
        public string? Appartament { get; set; }
        public string? PostalCode { get; set; }
        public string? Entrance { get; set; }
    }
}
