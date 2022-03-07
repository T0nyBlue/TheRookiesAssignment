using AutoMapper;
using DataAccess.Data;
using DataAccess.DTO.UserDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public UserController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        //Get all users
        [HttpGet]
        public async Task<ActionResult<List<UserReadDto>>> GetUsers(int page)
        {
            if (_db.Users == null)
            {
                return NotFound();
            }

            var pageResults = 3f;
            var pageCount = Math.Ceiling(_db.Users.Count() / pageResults);

            var userResponse = await _db.Users
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();

            var userDtoResponse = _mapper.Map<List<UserReadDto>>(userResponse);

            var response = new UserResponseDto
            {
                Users = userDtoResponse,
                CurrentPage = page,
                Pages = (int)pageCount
            };

            return Ok(response);
        }

        //Get one user
        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadDto>> GetOneUser(Guid id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.UserId == id);
            if (user == null)
            {
                return BadRequest("User not found!");
            }

            var userResponse = (from User in _db.Users
                                 join UserAddress in _db.UserAddresses
                                 on User.UserId equals UserAddress.UserId
                                 select new {
                                     UserId = User.UserId,
                                     UserName = User.UserName,
                                     FullName = User.FullName,
                                     DateofBirth = User.DateofBirth,
                                     PhoneNumber = User.PhoneNumber,
                                     Email = User.Email,
                                     Role = User.Role,
                                     UserAddrId = UserAddress.UserId,
                                     UserAddrPhoneNumber = UserAddress.UserAddrPhoneNumber,
                                     UserAddr = UserAddress.UserAddr,
                                     UserCity = UserAddress.UserCity,
                                     UserProvince = UserAddress.UserProvince,
                                 }).ToList();

            return Ok(userResponse);
        }

        //Search users
        [HttpGet("/api/User/Search")]
        public async Task<ActionResult<List<UserReadDto>>> SearchUsers(int page, string keyword)
        {
            if (_db.Users == null)
            {
                return NotFound();
            }

            var pageResults = 3f;
            var pageCount = Math.Ceiling(_db.Users.Count() / pageResults);

            var userResponse = await _db.Users
                .Where(x => x.UserName.Contains(keyword) || x.PhoneNumber.Contains(keyword))
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();

            var userDtoResponse = _mapper.Map<List<UserReadDto>>(userResponse);

            var response = new UserResponseDto
            {
                Users = userDtoResponse,
                CurrentPage = page,
                Pages = (int)pageCount
            };

            return Ok(response);
        }
    }
}
