namespace AddressService.Models
{
	/// <summary>
	/// Модель для десериализации ответа от DaData API
	/// </summary>
	public class CleanAddressResponse
	{
		public string? country { get; set; } // Страна
		public string? region { get; set; } // Регион
		public string? city { get; set; } // Город
		public string? street { get; set; } // Улица
		public string? house { get; set; } // Дом
		public string? flat { get; set; } // Квартира
		public string? postal_code { get; set; } // Почтовый индекс
		public string? entrance { get; set; } // Подъезд (если есть)
	}
}
