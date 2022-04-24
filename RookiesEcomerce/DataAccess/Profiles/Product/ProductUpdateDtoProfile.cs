using AutoMapper;
using DataAccess.DTO.ProductDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Profiles.Product
{
    public class ProductUpdateDtoProfile : Profile
    {
        public ProductUpdateDtoProfile()
        {
            CreateMap<ProductUpdateDto, DataAccess.Model.Product>();
        }
    }
}
