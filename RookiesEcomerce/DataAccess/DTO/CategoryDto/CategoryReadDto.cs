using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.CategoryDto
{
    public class CategoryReadDto
    {
        public Guid CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreateBy { get; set; }
    }
}
