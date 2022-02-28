using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Model
{
    public class Category
    {
        [Key]
        public Guid CategoryId { get; set; }
        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CategoryName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Guid CreateBy { get; set; }
        
        //Foreign Key
        public List<Product> Products { get; set; }
    }
}
