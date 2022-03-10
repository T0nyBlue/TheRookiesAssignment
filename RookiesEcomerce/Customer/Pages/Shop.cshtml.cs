using AutoMapper;
using DataAccess.Data;
using DataAccess.DTO.ProductDto;
using DataAccess.DTO.ProductImgDto;
using DataAccess.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Customer.Pages
{
    public class ShopModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        //public ProductResponseDto response { get; set; }

        public List<ProductReadDto> productDtoResponse { get; set; }

        public List<Product> productResponse { get; set; }

        public const int pageResults = 1;

        [BindProperty(SupportsGet = true, Name = "p")]
        public int page { get; set; } = 1;

        public int pagesCount { get; set; }

        public string searchstring { get; set; } = "";

        public ShopModel(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task OnGetAsync(string SearchString)
        {
            //pagesCount = (int)Math.Ceiling((double)_db.Products.Count() / pageResults);
            searchstring = SearchString;

            if (!string.IsNullOrEmpty(SearchString))
            {
                pagesCount = (int)Math.Ceiling((double)_db.Products.Where(x => x.ProductName.Contains(SearchString)).Count() / pageResults);
                productResponse = await _db.Products
                    .Where(x => x.ProductName.Contains(SearchString))
                    .Skip((page - 1) * (int)pageResults)
                    .Take((int)pageResults)
                    .ToListAsync();
            }
            else
            {
                pagesCount = (int)Math.Ceiling((double)_db.Products.Count() / pageResults);
                productResponse = await _db.Products
                    .Skip((page - 1) * (int)pageResults)
                    .Take((int)pageResults)
                    .ToListAsync();
            }

            if (page < 1)
            {
                page = 1;
            }
            if (page > pagesCount)
            {
                page = pagesCount;
            }


            productDtoResponse = _mapper.Map<List<ProductReadDto>>(productResponse);

            foreach (var product in productDtoResponse)
            {
                product.ProductImgReadDto = _mapper.Map<List<ProductImgReadDto>>(_db.ProductImgs.Where(x => x.ProductId == product.ProductId));
            }

        }
    }
}
