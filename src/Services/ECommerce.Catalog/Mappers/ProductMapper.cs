using AutoMapper;
using ECommerce.Catalog.Entities;
using ECommerce.Catalog.Models;

namespace ECommerce.Catalog.Mappers
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
        }
    }
}