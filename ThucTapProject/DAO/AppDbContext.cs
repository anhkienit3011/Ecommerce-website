using Microsoft.EntityFrameworkCore;
using ThucTapProject.Entities;
using ThucTapProject.EntityModel;

namespace ThucTapProject.DAO
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<Decentralization> Decentralization { get; set; }
        public virtual DbSet<Accountt> Accountt { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductImage> ProductImage { get; set; }
        public virtual DbSet<ProductType> ProductType { get; set; }
        public virtual DbSet<ProductReview> ProductReview { get; set; }
        public virtual DbSet<Carts> Carts { get; set; }
        public virtual DbSet<CartItem> CartItem { get; set; }
        public virtual DbSet<OrderStatus> OrderStatus { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=.\\DONGSQLSERVER;" +
                "database=ThucTap_WebBanHang; trusted_connection=true; " +
                "trustservercertificate=true; " +
                "MultipleActiveResultSets=true");
        }
    }
}
