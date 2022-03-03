using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Model
{
    public class ProductImg
    {
        [Key]
        public Guid ImgId { get; set; }
        public string? ProductImgLink { get; set; }

        //Foreign Key
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
