using AutoMapper;
using DataAccess.Data;
using DataAccess.DTO.ProductDto;
using DataAccess.DTO.ProductImgDto;
using DataAccess.Model;
using EnsureThat;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Admin.Controllers
{
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
        public async Task<ActionResult<List<ProductReadDto>>> GetProducts(int page)
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
                .ToListAsync();

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
        public async Task<ActionResult<List<ProductCreateDto>>> CreateProduct(ProductCreateDto postProductDto)
        {
            Ensure.Any.IsNotNull(postProductDto, nameof(postProductDto));
            var product = _mapper.Map<Product>(postProductDto);
            _db.Products.Add(product);

            foreach (var img in postProductDto.ProductImgCreateDto)
            {
                var productImg = _mapper.Map<ProductImg>(img);
                productImg.ProductId = product.ProductId;
                _db.ProductImgs.Add(productImg);
            }

            await _db.SaveChangesAsync();

            return Ok(product);
        }
    }
}
