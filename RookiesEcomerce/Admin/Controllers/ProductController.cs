using AutoMapper;
using DataAccess.Data;
using DataAccess.DTO.ProductDto;
using DataAccess.DTO.ProductImgDto;
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
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public ProductController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        //Get all products
        [HttpGet]
        public async Task<IActionResult> GetProducts(int page)
        {
            if (_db.Products == null)
            {
                return NotFound();
            }

            var pageResults = 3f;
            var pageCount = Math.Ceiling(_db.Products.Count() / pageResults);

            var productResponse = await _db.Products
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .Include(p => p.Category)
                .ToListAsync();

            var productDtoResponse = _mapper.Map<List<ProductReadDto>>(productResponse);

            //foreach (var product in productDtoResponse)
            //{
            //    product.ProductImgReadDto = _mapper.Map<List<ProductImgReadDto>>(_db.ProductImgs.Where(x => x.ProductId == product.ProductId));
            //}

            var response = new ProductResponseDto
            {
                Products = productDtoResponse,
                CurrentPage = page,
                Pages = (int)pageCount
            };

            return Ok(response);
        }

        //Get one products
        [HttpGet("{id}")]
        public async Task<ActionResult<List<ProductReadDto>>> GetOneProduct(string id)
        {
            var productResponse = _db.Products.FirstOrDefault(x => x.ProductId.ToString() == id);
            if(productResponse == null)
            {
                return NotFound("No Product founded!");
            }

            var productDtoResponse = _mapper.Map<ProductReadDto>(productResponse);
            productDtoResponse.ProductImgReadDto = _mapper.Map<List<ProductImgReadDto>>(_db.ProductImgs.Where(x => x.ProductId == productDtoResponse.ProductId));

            return Ok(productDtoResponse);
        }

        //Search all products
        [HttpGet("/api/Product/Search")]
        public async Task<ActionResult<List<ProductReadDto>>> SearchProducts(int page, string keyword, string categoryId)
        {
            if (_db.Products == null)
            {
                return NotFound();
            }

            if (keyword == "All" && categoryId != "All")
            {
                var pageResults = 3f;
                var pageCount = Math.Ceiling(_db.Products.Where(x => x.CategoryId.ToString() == categoryId).Count() / pageResults);

                var productResponse = await _db.Products
                    .Where(x => x.CategoryId.ToString() == categoryId)
                    .Skip((page - 1) * (int)pageResults)
                    .Take((int)pageResults)
                    .ToListAsync();

                if (productResponse == null)
                {
                    return NotFound();
                }

                var productDtoResponse = _mapper.Map<List<ProductReadDto>>(productResponse);

                foreach (var product in productDtoResponse)
                {
                    product.ProductImgReadDto = _mapper.Map<List<ProductImgReadDto>>(_db.ProductImgs.Where(x => x.ProductId == product.ProductId));
                }

                var response = new ProductResponseDto
                {
                    Products = productDtoResponse,
                    CurrentPage = page,
                    Pages = (int)pageCount
                };

                return Ok(response);
            }
            else if (keyword != "All" && categoryId == "All")
            {
                var pageResults = 3f;
                var pageCount = Math.Ceiling(_db.Products.Where(x => x.ProductName.Contains(keyword)).Count() / pageResults);

                var productResponse = await _db.Products
                    .Where(x => x.ProductName.Contains(keyword))
                    .Skip((page - 1) * (int)pageResults)
                    .Take((int)pageResults)
                    .Include(p => p.Category)
                    .ToListAsync();

                if (productResponse == null)
                {
                    return NotFound();
                }

                var productDtoResponse = _mapper.Map<List<ProductReadDto>>(productResponse);

                foreach (var product in productDtoResponse)
                {
                    product.ProductImgReadDto = _mapper.Map<List<ProductImgReadDto>>(_db.ProductImgs.Where(x => x.ProductId == product.ProductId));
                }

                var response = new ProductResponseDto
                {
                    Products = productDtoResponse,
                    CurrentPage = page,
                    Pages = (int)pageCount
                };

                return Ok(response);
            }
            else
            {
                var pageResults = 3f;
                var pageCount = Math.Ceiling(_db.Products.Where(x => x.CategoryId.ToString() == categoryId && x.ProductName.Contains(keyword)).Count() / pageResults);

                var productResponse = await _db.Products
                    .Where(x => x.CategoryId.ToString() == categoryId && x.ProductName.Contains(keyword))
                    .Skip((page - 1) * (int)pageResults)
                    .Take((int)pageResults)
                    .ToListAsync();

                if (productResponse == null)
                {
                    return NotFound();
                }

                var productDtoResponse = _mapper.Map<List<ProductReadDto>>(productResponse);

                foreach (var product in productDtoResponse)
                {
                    product.ProductImgReadDto = _mapper.Map<List<ProductImgReadDto>>(_db.ProductImgs.Where(x => x.ProductId == product.ProductId));
                }

                var response = new ProductResponseDto
                {
                    Products = productDtoResponse,
                    CurrentPage = page,
                    Pages = (int)pageCount
                };

                return Ok(response);
            }
        }

        //Create product
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductCreateDto postProductDto)
        {
            //Ensure.Any.IsNotNull(postProductDto, nameof(postProductDto));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = _mapper.Map<Product>(postProductDto);
            _db.Products.Add(product);

            if(postProductDto.ProductImgCreateDto != null)
            {
                foreach (var img in postProductDto.ProductImgCreateDto)
                {
                    var productImg = _mapper.Map<ProductImg>(img);
                    productImg.ProductId = product.ProductId;
                    _db.ProductImgs.Add(productImg);
                }
            }    

            await _db.SaveChangesAsync();

            return Ok(product);
        }

        //Update product
        [HttpPut("{id}")]
        public async Task<ActionResult<List<ProductCreateDto>>> UpdateProduct(Guid id, [FromBody]ProductUpdateDto postProductDto)
        {
            //Ensure.Any.IsNotNull(postProductDto, nameof(postProductDto));
            //var product = _mapper.Map<Product>(postProductDto);
            //_db.Products.Update(product);

            //var prId = Guid.Parse(postProductDto.ProductId);

            var product = _db.Products.Find(id);

            if (product == null)
                return BadRequest("No Product Not Founded!"); 

            product.ProductName = postProductDto.ProductName;
            product.ProductDescription = postProductDto?.ProductDescription;
            product.LastModifyDate = DateTime.Now;
            product.Price = postProductDto.Price;

            _db.Products.Update(product);
            await _db.SaveChangesAsync();

            if (postProductDto.ProductImgCreateDto.Count != 0)
            {
                foreach (var img in postProductDto.ProductImgCreateDto)
                {
                    //var productImg = _mapper.Map<ProductImg>(img);
                    //productImg.ProductId = product.ProductId;
                    var productImg = _db.ProductImgs.FirstOrDefault(x => x.ProductId == id);
                    productImg.ProductImgLink = img.ProductImgLink;
                    _db.ProductImgs.Update(productImg);
                }
            }

            await _db.SaveChangesAsync();

            return Ok(product);
        }

        //Delete
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            var product = await _db.Products.FirstOrDefaultAsync(x => x.ProductId == id);
            if (product == null)
            {
                return NotFound("No product founded!");
            }

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            return Ok("Category Deleted");
        }
    }
}
