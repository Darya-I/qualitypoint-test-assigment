using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dadata;
using Dadata.Model;
using AddressService.Services;
using AutoMapper;
using AddressService.Models;


namespace AddressService.Controllers
{
    /// <summary>
    /// контроллер для работы с адресами
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly DadataClient _dadataClient;


        public AddressController(IMapper mapper, DadataClient dadataClient)
        {
            _mapper = mapper;
            _dadataClient = dadataClient;
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

            var standardizedAddresses = await _dadataClient.CleanAddressAsync(address);

            if (standardizedAddresses == null || standardizedAddresses.Length == 0)
            {
                return NotFound(new { Message = "Unable to standardize the provided address." });
            }

            var response = _mapper.Map<AddressResponce>(standardizedAddresses[0]); // берём первый адрес из массива
            return Ok(response);
        }

    }

}
