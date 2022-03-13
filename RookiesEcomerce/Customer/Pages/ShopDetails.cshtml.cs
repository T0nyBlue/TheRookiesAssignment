using AutoMapper;
using DataAccess.Data;
using DataAccess.DTO.ProductDto;
using DataAccess.DTO.ProductImgDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Customer.Pages
{
    public class ShopDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        [BindProperty(SupportsGet = true, Name = "id")]
        public string Id { get; set; }

        public ProductReadDto productDtoResponse { get; set; }

        public ShopDetailsModel(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async void OnGet()
        {
            var productResponse = _db.Products.FirstOrDefault(x => x.ProductId.ToString() == Id);

            productDtoResponse = _mapper.Map<ProductReadDto>(productResponse);
            productDtoResponse.ProductImgReadDto = _mapper.Map<List<ProductImgReadDto>>(_db.ProductImgs.Where(x => x.ProductId == productDtoResponse.ProductId));
        }
    }
}
