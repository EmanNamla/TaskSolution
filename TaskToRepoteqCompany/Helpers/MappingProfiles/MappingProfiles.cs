using AutoMapper;
using TaskToRepoteqCompany.DAL.Models;
using TaskToRepoteqCompany.PL.ViewModels.Order;
using TaskToRepoteqCompany.PL.ViewModels.Product;

namespace TaskToRepoteqCompany.PL.Helpers.MappingProfiles
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductViewModel>().ReverseMap();

            CreateMap<Order, OrderViewModel>().ForMember(dest => dest.ProductIds, opt => opt.MapFrom(src => src.OrderProducts.Select(am => am.ProductId).ToList()))
            .ReverseMap();

        }
    }
}
