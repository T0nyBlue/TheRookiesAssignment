using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.OrderDto
{
    public class OrderResponseDto
    {
        public List<OrderReadDto> Orders { get; set; } = new List<OrderReadDto>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
