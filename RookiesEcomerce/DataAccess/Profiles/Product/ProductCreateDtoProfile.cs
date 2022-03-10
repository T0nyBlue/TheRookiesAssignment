using AutoMapper;
using DataAccess.DTO.ProductDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Profiles.Product
{
    public class ProductCreateDtoProfile : Profile
    {
        public ProductCreateDtoProfile()
        {
            CreateMap<ProductCreateDto, DataAccess.Model.Product >();
        }
    }
}
