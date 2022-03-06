using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.CategoryDto
{
    public class CategoryCreateDto
    {
        [Key]
        public Guid CategoryId { get; set; }
        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string? CategoryName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Guid CreateBy { get; set; }
    }
}
