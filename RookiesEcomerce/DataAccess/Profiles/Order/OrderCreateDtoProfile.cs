using DataAccess.DTO.OrderDto;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Profiles.Order
{
    public class OrderCreateDtoProfile : Profile
    {
        public OrderCreateDtoProfile()
        {
            CreateMap<OrderCreateDto, DataAccess.Model.Order>();
        }
    }
}
