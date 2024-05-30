using System.Configuration;
using cartingWithRedis.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;



namespace cartingWithRedis
{
    public class ApplicationDbContext : IdentityDbContext<Customer, IdentityRole<int>, int>
    {


        public ApplicationDbContext()
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);


            optionsBuilder.UseMySQL("server=localhost; database=Carting; user=; password=");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, ProductName = "Nike AirMax95", ProductUrl = "/Images/download2.jpeg", ProductPrice = 200 },
                new Product { Id = 2, ProductName = "Nike AirMax95", ProductUrl = "/Images/download3.jpeg", ProductPrice = 200 },
                new Product { Id = 3, ProductName = "Nike AirMax95", ProductUrl = "/Images/download4.jpeg", ProductPrice = 200 },
                new Product { Id = 4, ProductName = "Nike AirMax95", ProductUrl = "/Images/download5.jpeg", ProductPrice = 200 },
                new Product { Id = 5, ProductName = "Nike AirMax95", ProductUrl = "/Images/download6.jpeg", ProductPrice = 200 },
                new Product { Id = 6, ProductName = "Nike AirMax95", ProductUrl = "/Images/download7.jpeg", ProductPrice = 200 },
                new Product { Id = 7, ProductName = "Nike AirMax95", ProductUrl = "/Images/download.jpeg", ProductPrice = 200 },
                new Product { Id = 8, ProductName = "Nike AirMax95", ProductUrl = "/Images/s-l1200.jpeg", ProductPrice = 200 }

            );



            modelBuilder.Entity<Customer>().HasOne(x => x.Cart)
                        .WithOne(x => x.Customer)
                        .HasForeignKey<Cart>(x => x.CustomerId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cart>().HasMany(x => x.Products)
                        .WithMany(x => x.Carts)
                        .UsingEntity(x => x.ToTable("CartItems"));

        }
    }
}