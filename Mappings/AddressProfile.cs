using AddressService.Controllers;
using AutoMapper;
using Dadata.Model;

namespace AddressService.Mappings
{
    /// <summary>
    /// сопоставляет свойства модели Address из Dadata со свойствами AddressResponce
    /// </summary>
    public class AddressProfile : Profile
    {
        public AddressProfile() 
        {
            CreateMap<Address, AddressResponce>()
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.country))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.city))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.street))
                .ForMember(dest => dest.HouseNumber, opt => opt.MapFrom(src => src.house))
                .ForMember(dest => dest.Appartament, opt => opt.MapFrom(src => src.flat))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.postal_code))
                .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.region))
                .ForMember(dest => dest.Entrance, opt => opt.MapFrom(src => src.entrance));
                
        }
    }
}
