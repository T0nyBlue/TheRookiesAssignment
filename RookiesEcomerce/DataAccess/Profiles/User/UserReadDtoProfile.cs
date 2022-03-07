using AutoMapper;
using DataAccess.DTO.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Profiles.User
{
    public class UserReadDtoProfile : Profile
    {
        public UserReadDtoProfile()
        {
            CreateMap<DataAccess.Model.User, UserReadDto>();
        }
    }
}
