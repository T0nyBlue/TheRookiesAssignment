using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.CategoryDto
{
    public class CategoryResponseDto
    {
        public List<CategoryReadDto> Categories { get; set; } = new List<CategoryReadDto>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
