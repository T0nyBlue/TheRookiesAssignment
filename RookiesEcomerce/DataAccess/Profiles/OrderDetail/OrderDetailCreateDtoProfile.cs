using AutoMapper;
using DataAccess.DTO.OrderDetailDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Profiles.OrderDetail
{
    public class OrderDetailCreateDtoProfile : Profile
    {
        public OrderDetailCreateDtoProfile()
        {
            CreateMap<OrderDetailCreateDto, DataAccess.Model.OrderDetail>();
        }
    }
}
