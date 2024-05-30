using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using cartingWithRedis.IRepository;
using cartingWithRedis.Models;
using Microsoft.AspNetCore.Identity;

namespace cartingWithRedis.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;

        public CustomerService(UserManager<Customer> userManager, SignInManager<Customer> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<bool> CreateCustomerAsync(string Email, string UserName, string Password)
        {
            if (Email == null || UserName == null || Password == null)
            {
                throw new NullReferenceException("customer is null");
            }
            Customer customer = new()
            {
                Email = Email,
                UserName = UserName,
            };
            var result = await _userManager.CreateAsync(customer, Password);
            return result.Succeeded;
        }

        public async Task<bool> LoginAsync(string Email, string password)
        {
            var user = await _userManager.FindByEmailAsync(Email) ?? throw new NullReferenceException("user not found");
            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            return result.Succeeded;


        }

        public async Task LogOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}