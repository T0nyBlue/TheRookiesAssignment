using AutoMapper;
using DataAccess.DTO.MyUserDto;
using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Profiles.User
{
    public class UserCreateDtoProfile : Profile
    {
        public UserCreateDtoProfile()
        {
            CreateMap<CreateUserDto, MyUser>();
        }
    }
}
