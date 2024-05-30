using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace cartingWithRedis.Views.Cart
{
    public class Cart : PageModel
    {
        private readonly ILogger<Cart> _logger;

        public Cart(ILogger<Cart> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}