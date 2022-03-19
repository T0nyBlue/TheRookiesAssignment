using AutoMapper;
using DataAccess.DTO.ProductDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Profiles.Product
{
    public class ProductReadDtoProfile : Profile
    {
        public ProductReadDtoProfile()
        {
            CreateMap<DataAccess.Model.Product, ProductReadDto>();
        }
    }
}
