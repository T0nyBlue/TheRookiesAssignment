using DataAccess.DTO.ProductImgDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.ProductDto
{
    public class ProductUpdateDto
    {
        public Guid ProductId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ProductName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string? ProductDescription { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public float Price { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int ProductQuantity { get; set; }

        //[Required(AllowEmptyStrings = false)]
        //[DisplayFormat(ConvertEmptyStringToNull = false)]
        //public DateTime CreateDate { get; set; }

        //[Required(AllowEmptyStrings = false)]
        //[DisplayFormat(ConvertEmptyStringToNull = false)]
        //public DateTime LastModifyDate { get; set; } = DateTime.Now;
        public float TotalRating { get; set; } = 0;

        //Foreign Key
        public Guid CategoryId { get; set; }
        public List<ProductImgCreateDto>? ProductImgCreateDto { get; set; }
    }
}
