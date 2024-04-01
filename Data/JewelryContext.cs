using Jewelry.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

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
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }

        public DbSet<InventoryReceipt> InventoryReceipt { get; set; }

        public DbSet<InventoryReceiptDetails> InventoryReceiptDetails { get; set; }

        public DbSet<Warranty> Warranties { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<Material> Material { get; set; }
        public DbSet<Size> Size { get; set; }
        public DbSet<PurchasePrice> PurchasePrice { get; set; }
        public DbSet<SalesPrice> SalesPrice { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<StatusCategory> StatusCategory { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Purity> Purity { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_config["ConnectionStrings:JewelryContextDb"]);

        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    var size = Size.FirstOrDefault(s => s.Id == 1);
        //    var material = Material.FirstOrDefault(s => s.Id == 1);
        //    var product = Products.Where(p => p.Id == 9).FirstOrDefault();
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<SalesPrice>().HasData(
        //        new SalesPrice { Id = 1, Price = 20.0m, EffectiveDate = DateTime.UtcNow },
        //        new SalesPrice { Id = 2, Price = 25.0m, EffectiveDate = DateTime.UtcNow }
        //    );

        //    modelBuilder.Entity<PurchasePrice>().HasData(
        //        new PurchasePrice { Id = 1, Price = 10.0m, EffectiveDate = DateTime.UtcNow },
        //        new PurchasePrice { Id = 2, Price = 15.0m, EffectiveDate = DateTime.UtcNow }
        //    );
        //    modelBuilder.Entity<ProductItem>().HasData(
        //    new ProductItem
        //    {
        //        Product = product,
        //        Sizes = size,
        //        Materials = material,
        //        PurchasePrice = null,
        //        SalesPrice = null,
        //        Quantity = 100
        //    },
        //    new ProductItem
        //    {
        //        Product = product,
        //        Sizes = size,
        //        Materials = material,
        //        PurchasePrice = null,
        //        SalesPrice = null,
        //        Quantity = 150
        //    }
        //    );
        //}
    }
}
