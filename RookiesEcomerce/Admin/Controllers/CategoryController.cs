using AutoMapper;
using DataAccess.Data;
using DataAccess.DTO;
using DataAccess.DTO.CategoryDto;
using DataAccess.Model;
using EnsureThat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Admin.Controllers
{
    [Authorize(Roles = "Admin")]
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

        //Get all categories
        [HttpGet]
        public async Task<ActionResult<List<CategoryReadDto>>> GetCategories(int page)
        {
            if (_db.Categories == null)
            {
                return NotFound();
            }

            var pageResults = 3f;
            var pageCount = Math.Ceiling(_db.Categories.Count() / pageResults);

            var categoryResponse = await _db.Categories
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();

            var categoryDtoResponse = _mapper.Map<List<CategoryReadDto>>(categoryResponse);

            var response = new CategoryResponseDto
            {
                Categories = categoryDtoResponse,
                CurrentPage = page,
                Pages = (int)pageCount
            };

            return Ok(response);
        }

        //Get one categories
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryReadDto>> GetOneCategory(Guid id)
        {
            var category = await _db.Categories.FirstOrDefaultAsync(x => x.CategoryId ==id);
            if(category == null)
            {
                return BadRequest("Category not found!");
            }

            var categoryResponse = _mapper.Map<CategoryReadDto>(category);

            return Ok(categoryResponse);
        }

        //Search categories
        [HttpGet("/api/Category/Search")]
        public async Task<ActionResult<List<CategoryReadDto>>> SearchCategories(int page, string keyword)
        {
            if (_db.Categories == null)
            {
                return NotFound();
            }

            var pageResults = 3f;
            var pageCount = Math.Ceiling(_db.Categories.Where(x => x.CategoryName.Contains(keyword)).Count() / pageResults);

            var categoryResponse = await _db.Categories
                .Where(x => x.CategoryName.Contains(keyword))
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();

            var categoryDtoResponse = _mapper.Map<List<CategoryReadDto>>(categoryResponse);

            var response = new CategoryResponseDto
            {
                Categories = categoryDtoResponse,
                CurrentPage = page,
                Pages = (int)pageCount
            };

            return Ok(response);
        }

        //Create category
        [HttpPost]
        public async Task<ActionResult<List<CategoryCreateDto>>> CreateCategory(CategoryCreateDto postCategoryDto)
        {
            Ensure.Any.IsNotNull(postCategoryDto, nameof(postCategoryDto));
            var category = _mapper.Map<Category>(postCategoryDto);

            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            return Ok(category);
        }

        //Update categories
        [HttpPut]
        public async Task<ActionResult<CategoryUpdateDto>> UpdateCategory(CategoryUpdateDto updateCategoryDto)
        {
            Ensure.Any.IsNotNull(updateCategoryDto, nameof(updateCategoryDto));
            var category = _mapper.Map<Category>(updateCategoryDto);

            _db.Categories.Update(category);
            await _db.SaveChangesAsync();

            return Ok("Category Updated!");
        }


        //Delete
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(Guid id)
        {
            var category = await _db.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);
            if(category == null)
            {
                return NotFound("No category founded!");
            }

            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();

            return Ok("Category Deleted");
        }
    }
}
