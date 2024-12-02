namespace AddressService.Models
{
    /// <summary>
    /// модель для десериализации ответа от DaData 
    /// </summary>
    public class CleanAddress
    {
        public string? country { get; set; }
        public string? region { get; set; }
        public string? city { get; set; }
        public string? street { get; set; }
        public string? house { get; set; }
        public string? flat { get; set; }
        public string? postal_code { get; set; }
        public string? entrance { get; set; }
    }
}
