using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dadata;
using Dadata.Model;
using AddressService.Services;
using AutoMapper;


namespace AddressService.Controllers
{
    /// <summary>
    /// контроллер для работы с адресами
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IDadataService _dadataService;
        private readonly ILogger<AddressController> _logger;
        private readonly IMapper _mapper;

        public AddressController(IDadataService dadataService, IMapper mapper, ILogger<AddressController> logger)
        {
            _dadataService = dadataService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        ///  гет-эндпоинт 
        /// </summary>
        /// <param name="address"> одна входная строка с адресом</param>
        /// <returns>
        /// 400 - пустой/не указан
        /// 404 - не удалось стандартизировать
        /// 200 - успех
        /// </returns>

        [HttpGet("standardize")]
        public async Task<IActionResult> StandardizeAddress([FromQuery] string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                return BadRequest(new { Message = "Address cannot be null or empty." });
            }

            var standardizedAddress = await _dadataService.StandardizeAddressAsync(address);

            if (standardizedAddress == null)
            {
                return NotFound(new { Message = "Unable to standardize the provided address." });
            }

            var response = _mapper.Map<AddressResponce>(standardizedAddress);
            return Ok(response);
        }
    }

  

    /// <summary>
    /// выходная модель ответа 
    /// </summary>
    public class AddressResponce
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string Appartament { get; set; }
        public string Entrance { get; set; } 
        public string PostalCode { get; set; }
        public string Region { get; set; }
    }
}
