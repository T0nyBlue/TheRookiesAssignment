using DataAccess.Data;
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
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
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
    }
}
