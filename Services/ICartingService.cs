using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cartingWithRedis.Models;

namespace cartingWithRedis.Services
{
    public interface ICartingService
    {
        Task<IEnumerable<Product?>> GetCartByIdAsync();
        Task AddToCartAsync(int productId, CancellationToken cancellationToken);
        Task RemoveProductFromCart(int ProductId, string customerId, CancellationToken cancellationToken);
        Task Updatecart(Product product, string customerId, CancellationToken cancellationToken);
    }
}