using AutoMapper;
using Customer.Helpers;
using DataAccess.Data;
using DataAccess.DTO.AddToCartDto;
using DataAccess.DTO.OrderDto;
using DataAccess.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Customer.Pages
{
    [Authorize]
    public class CheckOutcshtmlModel : PageModel
    {
        //private readonly IMetaIdentityUserService _metaIdentityUserService;
        //private readonly IOrderService _orderService;
        //private readonly IOrderItemService _orderItemService;
        //private readonly IProductRatingService _productRatingService;

        //public CheckoutModel(IMetaIdentityUserService metaIdentityUserService, IOrderService orderService,
        //    IOrderItemService orderItemService, IProductRatingService productRatingService)
        //{
        //    _metaIdentityUserService = metaIdentityUserService;
        //    _orderService = orderService;
        //    _orderItemService = orderItemService;
        //    _productRatingService = productRatingService;
        //}

        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public CheckOutcshtmlModel(UserManager<MyUser> userManager, ApplicationDbContext db, IMapper mapper)
        {
            UserManager = userManager;
            _db = db;
            _mapper = mapper;
        }

        public List<AddToCartDto> Cart { get; set; }
        public MyUser CurrentUser { get; set; }

        [BindProperty]
        public OrderCreateDto CreateOrder { get; set; }

        public decimal TotalMoney { get; set; }
        public UserManager<MyUser> UserManager { get; }

        public async Task<IActionResult> OnGet()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToPage("/Authentication/Login");

            Cart = SessionHelper.GetObjectFromJson<List<AddToCartDto>>(HttpContext.Session, "cart");
            string userId = User.Claims.FirstOrDefault(u => u.Type == "sub").Value;
            CurrentUser = await UserManager.FindByIdAsync(userId);
            CreateOrder = new OrderCreateDto
            {
                FirstName = CurrentUser.FirstName,
                LastName = CurrentUser.LastName,
                Country = CurrentUser.Country,
                Line1 = CurrentUser.Line1,
                Line2 = CurrentUser.Line2,
                PhoneNumber = CurrentUser.PhoneNumber,
                Province = CurrentUser.Province,
                Total = Cart.Sum(p => p.Quantity * p.Product.Price)
            };
            if (Cart == null)
                return RedirectToPage("/Home/Index");
            TotalMoney = (decimal)Cart.Sum(p => p.Quantity * p.Product.Price);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                Cart = SessionHelper.GetObjectFromJson<List<AddToCartDto>>(HttpContext.Session, "cart");

                if (Cart != null)
                {
                    Guid userId = Guid.Parse(User.Claims.FirstOrDefault(u => u.Type == "sub").Value);
                    CreateOrder.Total = Cart.Sum(p => p.Quantity * p.Product.Price);
                    CreateOrder.Status = "Success";
                    CreateOrder.UserId = userId;

                    var order = _mapper.Map<Order>(CreateOrder);

                    _db.Orders.Add(order);
                    _db.SaveChanges();
                }
                TempData["AlertMessage"] = "Order Product Successfully!";
                SessionHelper.Remove(HttpContext.Session, "cart");
                return RedirectToPage("/Shop");
            }

            return Page();
        }
    }
}
