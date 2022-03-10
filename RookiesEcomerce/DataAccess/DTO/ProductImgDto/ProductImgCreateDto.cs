using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.ProductImgDto
{
    public class ProductImgCreateDto
    {
        public string? ProductImgLink { get; set; }

        //Foreign Key
        //public Guid? ProductId { get; set; }
    }
}
