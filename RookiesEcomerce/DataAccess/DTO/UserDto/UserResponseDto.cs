using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.UserDto
{
    public class UserResponseDto
    {
        public List<UserReadDto> Users { get; set; } = new List<UserReadDto>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
