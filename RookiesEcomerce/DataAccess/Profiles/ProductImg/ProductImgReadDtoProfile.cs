using AutoMapper;
using DataAccess.DTO.ProductImgDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Profiles.ProductImg
{
    public class ProductImgReadDtoProfile : Profile
    {
        public ProductImgReadDtoProfile()
        {
            CreateMap<DataAccess.Model.ProductImg, ProductImgReadDto>();
        }
    }
}
