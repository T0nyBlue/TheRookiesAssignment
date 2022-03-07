using AutoMapper;
using DataAccess.DTO.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Profiles.Order
{
    public class OrderReadDtoProfile : Profile
    {
        public OrderReadDtoProfile()
        {
            CreateMap<DataAccess.Model.Order, OrderReadDto>();
        }
    }
}
