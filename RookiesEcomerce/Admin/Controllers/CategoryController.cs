using AutoMapper;
using DataAccess.Data;
using DataAccess.DTO;
using DataAccess.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public CategoryController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetAllCategories()
        {
            return Ok(await _db.Categories.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Category>>> GetOneCategory(Guid id)
        {
            var category = await _db.Categories.FindAsync(id);
            if(category == null)
            {
                return BadRequest("Category not found!");
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<List<PostCategoryDto>>> CreateCategory(PostCategoryDto postCategoryDto)
        {
            //Mapping to Persist to Data
            var category = _mapper.Map<Category>(postCategoryDto);

            _db.Add(category);
            await _db.SaveChangesAsync();

            return Ok(category);
        }


    }
}
