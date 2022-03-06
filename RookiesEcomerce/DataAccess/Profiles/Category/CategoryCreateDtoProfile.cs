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
    public class CategoryCreateDtoProfile : Profile
    {
        public CategoryCreateDtoProfile()
        {
            CreateMap<CategoryCreateDto, DataAccess.Model.Category>();
        }
    }
}
