using AutoMapper;
using DataAccess.DTO;
using DataAccess.DTO.CategoryDto;
using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Profiles.Category
{
    public class CategoryReadDtoProfile : Profile
    {
        public CategoryReadDtoProfile()
        {
            CreateMap<DataAccess.Model.Category, CategoryReadDto>();
        }
    }
}
