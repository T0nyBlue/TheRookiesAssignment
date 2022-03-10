using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccess.DTO.OrderDto
{
    public class OrderReadDto
    {
        public Guid OrderId { get; set; }
        public Guid CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public float Total { get; set; }
        public string? Status { get; set; }

        //Foreign Key
        public Guid UserId { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }

    }
}
