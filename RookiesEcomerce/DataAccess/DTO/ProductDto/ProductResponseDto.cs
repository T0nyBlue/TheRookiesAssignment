using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.ProductDto
{
    public class ProductResponseDto
    {
        public List<ProductReadDto> Products { get; set; } = new List<ProductReadDto>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
