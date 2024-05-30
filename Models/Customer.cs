using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace cartingWithRedis.Models
{
    public class Customer : IdentityUser<int>
    {
        public string? Name { get; set; }
        public virtual Cart? Cart { get; set; }



    }
}