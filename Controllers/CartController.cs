using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using cartingWithRedis.IRepository;
using cartingWithRedis.Models;
using cartingWithRedis.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace cartingWithRedis.Controllers
{
    [Route("[controller]")]
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly ICartingService _cartService;
        private readonly IRepository<Product> _productRepo;
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICustomerService _customerService;


        public CartController(ILogger<CartController> logger,
                        ICartingService cartService,
                        IRepository<Product> productRepo,
                        UserManager<Customer> userManager,
                        IHttpContextAccessor httpContextAccessor,
                        ApplicationDbContext dbContext,
                        SignInManager<Customer> signInManager,
                        ICustomerService customerService)
        {
            _logger = logger;
            _cartService = cartService;
            _productRepo = productRepo;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            _signInManager = signInManager;
            _customerService = customerService;

        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(string email, string username, string password)
        {

            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return RedirectToAction("Login");
            }
            var result = await _customerService.CreateCustomerAsync(email, username, password);
            return result ? RedirectToAction("Login") : View();
        }

        [Route("Dashboard")]
        public async Task<IActionResult> DashboardAsync()
        {

            var products = await _productRepo.GetAllAsync();
            if (products == null || !products.Any())
            {
                return NotFound();
            }
            return View(products);
        }

        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet("Login")]
        public async Task<IActionResult> Login(string email, string password)
        {

            if (ModelState.IsValid)
            {
                var result = await _customerService.LoginAsync(email, password);
                return result ? RedirectToAction("Dashboard") : View();
            }
            ModelState.AddModelError("", "password or email invalid");
            return View();



        }

        [Route("AddToCart")]
        public IActionResult AddToCart()
        {
            return View();
        }

        [HttpGet("AddToCart/{productId:int}")]
        public async Task<IActionResult> AddToCartAsync([FromRoute] int productId, CancellationToken cancellationToken)
        {
            await _cartService.AddToCartAsync(productId, cancellationToken);
            return Ok();
        }

        [Route("cart")]
        public IActionResult Cart()
        {
            return View();
        }

        [HttpGet("cart")]
        public async Task<IActionResult> CartAsync()
        {
            var result = await _cartService.GetCartByIdAsync();
            return View(result);
        }





    }
}
