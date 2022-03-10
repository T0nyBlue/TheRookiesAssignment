using AutoMapper;
using DataAccess.DTO.ProductImgDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Profiles.ProductImg
{
    public class ProductImgCreateDtoProfile : Profile
    {
        public ProductImgCreateDtoProfile()
        {
            CreateMap<ProductImgCreateDto, DataAccess.Model.ProductImg>();
        }
    }
}
