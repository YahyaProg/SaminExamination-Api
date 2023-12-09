

using AutoMapper;
using SaminExamination.Dto;
using SaminExamination.Models;

namespace SaminExamination.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductUpdateDto, Product>();
            CreateMap<CategoryUpdateDto, Category>();
            CreateMap<ProductDto, Product>();
            CreateMap<Product, Product>();
            CreateMap<Category, CategoryDto>();
            CreateMap<BuyInvoiceDto , BuyInvoice>();
            CreateMap<InvoiceSellDto, InvoiceSell>();
            CreateMap<UserDto, User>();
        }
    }
}
