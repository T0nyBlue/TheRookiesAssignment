using DataAccess.DTO.ProductImgDto;
using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccess.DTO.ProductDto
{
    public class ProductReadDto
    {
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public float Price { get; set; }
        public int ProductQuantity { get; set; }
        public Guid CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid LastModifyBy { get; set; }
        public DateTime LastModifyDate { get; set; }
        public float TotalRating { get; set; } = 0;

        //Foreign Key
        public Guid CategoryId { get; set; }
        public List<ProductImgReadDto>? ProductImgReadDto { get; set; }
        //public List<ProductRating>? ProductRatings { get; set; }
    }
}
