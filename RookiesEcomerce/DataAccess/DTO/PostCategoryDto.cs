using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class PostCategoryDto
    {
        public string? CategoryName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Guid CreateBy { get; set; }
    }
}
