using AutoMapper;
using Customer.Helpers;
using DataAccess.Data;
using DataAccess.DTO.AddToCartDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Customer.Pages
{
    public class ShoppingCartModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public ShoppingCartModel(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<AddToCartDto> Cart { get; set; }
        public decimal Total { get; set; }

        public void OnGet()
        {
            Cart = SessionHelper.GetObjectFromJson<List<AddToCartDto>>(HttpContext.Session, "cart");
            if (Cart != null)
                Total = (decimal)Cart.Sum(i => i.Product.Price * i.Quantity);
            else
                Total = 0;
        }

        public IActionResult OnGetDelete(Guid productId)
        {
            Cart = SessionHelper.GetObjectFromJson<List<AddToCartDto>>(HttpContext.Session, "cart");
            int index = Exists(Cart, productId);
            if (index != -1)
                Cart.RemoveAt(index);

            if (Cart.Count > 0)
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", Cart);
            else
                SessionHelper.Remove(HttpContext.Session, "cart");
            return Page();
        }

        public IActionResult OnPostUpdateCart(int[] quantities)
        {
            Cart = SessionHelper.GetObjectFromJson<List<AddToCartDto>>(HttpContext.Session, "cart");
            if (Cart != null)
            {
                for (var i = 0; i < Cart.Count; i++)
                {
                    Cart[i].Quantity = quantities[i];
                }

                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", Cart);
            }
            return Page();
        }

        private int Exists(List<AddToCartDto> cart, Guid productId)
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
