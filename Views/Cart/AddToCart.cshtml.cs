using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace cartingWithRedis.Views.Cart
{
    public class AddToCart : PageModel
    {
        private readonly ILogger<AddToCart> _logger;

        public AddToCart(ILogger<AddToCart> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}