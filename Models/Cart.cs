using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cartingWithRedis.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        public virtual ICollection<Product>? Products { get; set; }

    }
}