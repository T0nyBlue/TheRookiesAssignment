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
                Province = CurrentUser.Province
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

                    //OrderDto order = await _orderService.CreateOrder(CreateOrder);
                    //List<CreateOrderItemDto> createOrderItemDtos = new List<CreateOrderItemDto>();
                    //List<CreateProductRatingDto> createProductRatingDtos = new List<CreateProductRatingDto>();
                    //foreach (var item in Cart)
                    //{
                    //    createOrderItemDtos.Add(new CreateOrderItemDto
                    //    {
                    //        OrderId = order.Id,
                    //        Price = item.Product.Price,
                    //        Quantity = item.Quantity,
                    //        ProductId = item.Product.Id,
                    //        CreatedBy = userId,
                    //        UpdatedBy = userId
                    //    });

                    //    // init temporary
                    //    createProductRatingDtos.Add(new CreateProductRatingDto
                    //    {
                    //        ProductId = item.Product.Id,
                    //        OrderItemId = order.Id,
                    //        IsRated = false,
                    //        Comment = "",
                    //        Rating = 5,
                    //        CreatedBy = userId,
                    //        UpdatedBy = userId
                    //    });
                    //}

                    //List<OrderItemDto> orderItemDtos = await _orderItemService.AddRangeOrderItemsAsync(createOrderItemDtos);

                    //for (int i = 0; i < orderItemDtos.Count; i++)
                    //{
                    //    createProductRatingDtos[i].OrderItemId = orderItemDtos[i].Id;
                    //}

                    //await _productRatingService.AddRangeProductRatingAsync(createProductRatingDtos);
                    //TempData["AlertMessage"] = "Order Product Successfully!";
                    //SessionHelper.Remove(HttpContext.Session, "cart");
                    //Cart = null;
                }
            }

            return Page();
        }
    }
}
