using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cartingWithRedis.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string? ProductName { get; set; }

        [Required]
        public string? ProductUrl { get; set; }

        [Required]
        [DisplayName("price")]
        public double ProductPrice { get; set; }

        public virtual ICollection<Cart>? Carts { get; set; }






    }
}