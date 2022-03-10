using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.OrderDetailDto
{
    public class OrderDetailReadDto
    {
        public Guid OrderDetailId { get; set; }
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
        public float TotalPrice { get; set; }

        //Foreign Key
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; } 
    }
}
