using DataAccess.DTO.ProductDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.AddToCartDto
{
    public class AddToCartDto
    {
        public ProductReadDto Product { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The Quantity should be greater than 1")]
        public int Quantity { get; set; }
    }
}
