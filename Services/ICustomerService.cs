using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cartingWithRedis.Models;

namespace cartingWithRedis.Services
{
    public interface ICustomerService
    {
        Task<bool> CreateCustomerAsync(string Email, string UserName, string password);
        Task<bool> LoginAsync(string Email, string password);
        Task LogOutAsync();
    }
}