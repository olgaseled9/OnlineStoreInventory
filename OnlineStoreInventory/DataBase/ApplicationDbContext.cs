using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OnlineStoreInventory.DataBase
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Supply> Supplies { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);  // Это необходимо для Identity

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Stock>()
                .HasOne(s => s.Product)
                .WithMany(p => p.Stock)
                .HasForeignKey(s => s.ProductId);

            modelBuilder.Entity<Supply>()
                .HasOne(s => s.Product)
                .WithMany(p => p.Supply)
                .HasForeignKey(s => s.ProductId);

            modelBuilder.Entity<Shipment>()
                .HasOne(sh => sh.Product)
                .WithMany(p => p.Shipments)
                .HasForeignKey(sh => sh.ProductId);

            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(po => po.Product)
                .WithMany(p => p.PurchaseOrders)
                .HasForeignKey(po => po.ProductId);
        }
    }
}
