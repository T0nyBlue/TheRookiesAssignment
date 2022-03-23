using AutoMapper;
using Customer.Helpers;
using DataAccess.Data;
using DataAccess.DTO.AddToCartDto;
using DataAccess.DTO.ProductDto;
using DataAccess.DTO.ProductImgDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Customer.Pages
{
    public class ShopDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public ShopDetailsModel(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }


        [BindProperty(SupportsGet = true, Name = "id")]
        public string Id { get; set; }

        public ProductReadDto productDtoResponse { get; set; }

        public ProductReadDto productAddToCart { get; set; }

        public List<AddToCartDto> Cart { get; set; }

        public void OnGet()
        {
            var productResponse = _db.Products.FirstOrDefault(x => x.ProductId.ToString() == Id);

            productDtoResponse = _mapper.Map<ProductReadDto>(productResponse);
            productDtoResponse.ProductImgReadDto = _mapper.Map<List<ProductImgReadDto>>(_db.ProductImgs.Where(x => x.ProductId == productDtoResponse.ProductId));
        }

        public async Task<ActionResult> OnPostBuyNow(Guid id, int ProductQty)
        {
            var productResponse = await _db.Products.FindAsync(id);

            productAddToCart = _mapper.Map<ProductReadDto>(productResponse);
            productAddToCart.ProductImgReadDto = _mapper.Map<List<ProductImgReadDto>>(_db.ProductImgs.Where(x => x.ProductId == productAddToCart.ProductId));
            
            if (productAddToCart != null)
            {
                Cart = SessionHelper.GetObjectFromJson<List<AddToCartDto>>(HttpContext.Session, "cart");
                if (Cart == null)
                {
                    Cart = new List<AddToCartDto>
                    {
                        new AddToCartDto
                        {
                            Product = productAddToCart,
                            Quantity = ProductQty
                        }
                    };
                }
                else
                {
                    int index = Exists(Cart, productAddToCart.ProductId);
                    if (index == -1)
                    {
                        Cart.Add(new AddToCartDto
                        {
                            Product = productAddToCart,
                            Quantity = ProductQty
                        });
                    }
                    else
                    {
                        Cart[index].Quantity += ProductQty;
                    }
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", Cart);
           
                return RedirectToPage("/ShopDetails", new { id = productAddToCart.ProductId });
            }
            return NotFound();
        }

        private static int Exists(List<AddToCartDto> cart, Guid productId)
        {
            for (var i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.ProductId == productId)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
