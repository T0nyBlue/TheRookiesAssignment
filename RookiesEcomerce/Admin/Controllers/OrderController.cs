using AutoMapper;
using DataAccess.Data;
using DataAccess.DTO.OrderDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public OrderController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        //Get all Orders
        [HttpGet]
        public async Task<ActionResult<List<OrderReadDto>>> GetOrders(int page)
        {
            if (_db.Orders == null)
            {
                return NotFound();
            }

            var pageResults = 3f;
            var pageCount = Math.Ceiling(_db.Orders.Count() / pageResults);

            var orderResponse = await _db.Orders
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();

            var orderDtoResponse = _mapper.Map<List<OrderReadDto>>(orderResponse);

            var response = new OrderResponseDto
            {
                Orders = orderDtoResponse,
                CurrentPage = page,
                Pages = (int)pageCount
            };

            return Ok(response);
        }

        //Search Orders
        [HttpGet("/api/Order/Search")]
        public async Task<ActionResult<List<OrderReadDto>>> SearchOrders(int page, string keyword)
        {
            if (_db.Orders == null)
            {
                return NotFound();
            }

            var pageResults = 3f;
            var pageCount = Math.Ceiling(_db.Orders.Where(x => x.OrderId.ToString().Contains(keyword)).Count() / pageResults);

            var orderResponse = await _db.Orders
                .Where(x => x.OrderId.ToString().Contains(keyword))
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();

            var orderDtoResponse = _mapper.Map<List<OrderReadDto>>(orderResponse);

            var response = new OrderResponseDto
            {
                Orders = orderDtoResponse,
                CurrentPage = page,
                Pages = (int)pageCount
            };

            return Ok(response);
        }
    }
}
