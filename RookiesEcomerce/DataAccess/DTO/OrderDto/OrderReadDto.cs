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

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Line1 { get; set; }
        public string? Line2 { get; set; }

        public string Province { get; set; }

        public string Country { get; set; }


        public DateTime CreateDate { get; set; }

        public float Total { get; set; }

        public string? Status { get; set; }

        public string UserId { get; set; }

        //Foreign Key
        public List<OrderDetail>? OrderDetails { get; set; }

    }
}
