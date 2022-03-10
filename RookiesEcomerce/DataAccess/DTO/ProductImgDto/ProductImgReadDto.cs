using DataAccess.DTO.ProductDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.ProductImgDto
{
    public class ProductImgReadDto
    {
        public Guid ImgId { get; set; }
        public string? ProductImgLink { get; set; }

        //Foreign Key
        public Guid ProductId { get; set; }
    }
}
