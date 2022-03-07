using AutoMapper;
using DataAccess.DTO.CategoryDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Profiles.Category
{
    public class CategoryUpdateDtoProfile : Profile
    {
        public CategoryUpdateDtoProfile()
        {
            CreateMap<CategoryUpdateDto, DataAccess.Model.Category>();
        }
    }
}
