using Jewelry.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Jewelry.Data
{
    //IdentityDbContext<StoreUser>
    public class JewelryContext : IdentityDbContext<StoreUser>
    {
        private readonly IConfiguration _config;

        //public JewelryContext(DbContextOptions<JewelryContext> options):base(options)
        public JewelryContext(IConfiguration config) 
        {
            _config = config;
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_config["ConnectionStrings:JewelryContextDb"]);

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Order>().HasData(new Order()
            //{
            //    Id = 1,
            //    OrderDate = DateTime.UtcNow,
            //    OrderNumber = "12345"
            //});
        }
        
        
    }
}
