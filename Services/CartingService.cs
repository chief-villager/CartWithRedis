using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using cartingWithRedis.IRepository;
using cartingWithRedis.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace cartingWithRedis.Services
{
    public class CartingService : ICartingService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Cart> _cartRepo;
        private readonly IConnectionMultiplexer _redisConnection;
        private readonly UserManager<Customer> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public CartingService(
            IRepository<Product> productRepo,
             UserManager<Customer> userManager,
             ApplicationDbContext dbContext,
             IHttpContextAccessor httpContextAccessor,
             IRepository<Cart> cartRepo)
        {
            _productRepository = productRepo;
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            _redisConnection = redis;
            _userManager = userManager;
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _cartRepo = cartRepo;

        }
        public async Task AddToCartAsync(int productId, CancellationToken cancellationToken)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var db = _redisConnection.GetDatabase();
                var product = productId <= 0
                                 ? throw new ArgumentOutOfRangeException(nameof(productId))
                                 : await _productRepository.GetByIdAsync(productId)
                                 ?? throw new NullReferenceException("products not found");
                string JsonProduct = JsonSerializer.Serialize(product);
                var hashEntries = new List<HashEntry>()
                  {
                    new($"product:{productId}",JsonProduct)
                  };
                var customerId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new NullReferenceException("Customer ID not found in HttpContext");
                await db.HashSetAsync($"customer:{customerId}", hashEntries.ToArray());
                Cart cart = new()
                {
                    CustomerId = int.Parse(customerId)
                };
                await _cartRepo.AddAsync(cart, cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (System.Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw new Exception(ex.Message);
            }

        }

        public async Task<IEnumerable<Product?>> GetCartByIdAsync()
        {
            var db = _redisConnection.GetDatabase();
            var customerId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new NullReferenceException("Customer ID not found in HttpContext");
            var cart = await db.HashGetAllAsync($"customer:{customerId}");
            List<Product?> products = [];

            foreach (var item in cart)
            {
                _ = item.Name;
                var jsonProduct = item.Value;
                var product = JsonSerializer.Deserialize<Product>(jsonProduct!);
                if (product != null)
                {
                    products.Add(product);
                }
            }
            return products;

        }
        public Task RemoveProductFromCart(int ProductId, string customerId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();

        }

        public Task Updatecart(Product product, string customerId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}