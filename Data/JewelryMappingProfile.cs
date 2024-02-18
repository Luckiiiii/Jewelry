using AutoMapper;
using Jewelry.Data.Entities;
using Jewelry.Models;

namespace Jewelry.Data
{
    public class JewelryMappingProfile : Profile
    {
        public JewelryMappingProfile() 
        {
            CreateMap<Order, OrderViewModel>()
                .ForMember(o => o.OrderId, ex => ex.MapFrom(o=>o.Id))
                .ReverseMap();
            CreateMap<OrderItem, OrderItemViewModel>()
                .ReverseMap()
                .ForMember(m => m.Product, opt => opt.Ignore());
        }
    }
}
