using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Website_Ecommerce.API.Data.Entities;
using Website_Ecommerce.API.Repositories;

namespace Website_Ecommerce.API.Data
{
    public class DataContext : DbContext, IUnitOfWork
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<VoucherOrder> VoucherOrders { get; set; }
        public DbSet<VoucherProduct> VoucherProducts { get; set; }


        public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            return await SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Product
            modelBuilder.Entity<Product>()
                .HasOne<Shop>(p => p.Shop)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.ShopId);
            
            //ProductCategory
            modelBuilder.Entity<ProductCategory>().HasKey(pc => new { pc.ProductId, pc.CategoryId});
            modelBuilder.Entity<ProductCategory>()
                .HasOne<Product>(pc => pc.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(pc => pc.ProductId);
            modelBuilder.Entity<ProductCategory>()
                .HasOne<Category>(pc => pc.Category)
                .WithMany(c => c.ProductCategories)
                .HasForeignKey(pc => pc.CategoryId);

            //ProductDetail
            modelBuilder.Entity<ProductDetail>()
                .HasOne<Product>(pd => pd.Product)
                .WithMany(p => p.ProductDetails)
                .HasForeignKey(pd => pd.ProductId);

            //ProdcutImage
            modelBuilder.Entity<ProductImage>()
                .HasOne<ProductDetail>(pi => pi.ProductDetail)  
                .WithMany(pd => pd.ProductImages)
                .HasForeignKey(pi => pi.ProductDetailId);

            //OrderDetail
            modelBuilder.Entity<OrderDetail>().HasKey(od => new { od.OrderId, od.ProductDetailId});
            modelBuilder.Entity<OrderDetail>()
                .HasOne<Order>(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId);
            modelBuilder.Entity<OrderDetail>()
                .HasOne<ProductDetail>(od => od.ProductDetail)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.ProductDetailId);
            modelBuilder.Entity<OrderDetail>()
                .HasOne<VoucherProduct>(od => od.VoucherProduct)
                .WithMany(vs => vs.OrderDetails)
                .HasForeignKey(od => od.VoucherProductId);
            
            //UserRole
            modelBuilder.Entity<UserRole>().HasKey(ur => new { ur.UserId, ur.RoleId});
            modelBuilder.Entity<UserRole>()
                .HasOne<User>(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);
            modelBuilder.Entity<UserRole>()
                .HasOne<Role>(ur => ur.Role)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            //Payment
            modelBuilder.Entity<Payment>()
                .HasOne<PaymentMethod>(p => p.PaymentMethod)
                .WithMany(pm => pm.Payments)
                .HasForeignKey(p => p.PaymentMethodId);
            modelBuilder.Entity<Payment>()
                .HasOne<Order>(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderId);
            
            //Order
            modelBuilder.Entity<Order>()
                .HasOne<VoucherOrder>(o => o.VoucherOrder)
                .WithMany(v => v.Orders)
                .HasForeignKey(o => o.VoucherId);
            modelBuilder.Entity<Order>()
                .HasOne<User>(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            //VoucherProduct
            modelBuilder.Entity<VoucherProduct>()
                .HasOne<Shop>(vp => vp.Shop)
                .WithMany(p => p.VoucherProducts)
                .HasForeignKey(vp => vp.ShopId);

            //Comment
            modelBuilder.Entity<Comment>()
                .HasOne<Product>(cm => cm.Product)
                .WithMany(p => p.Comments)
                .HasForeignKey(cm => cm.ProductId);
            modelBuilder.Entity<Comment>()
                .HasOne<User>(cm => cm.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(cm => cm.UserId);

            //Cart
            modelBuilder.Entity<Cart>()
                .HasOne<ProductDetail>(ct => ct.ProductDetail)
                .WithMany(p => p.Carts)
                .HasForeignKey(ct => ct.ProductDetailId);
            modelBuilder.Entity<Cart>()
                .HasOne<User>(ct => ct.User)
                .WithMany(u => u.Carts)
                .HasForeignKey(ct => ct.UserId);    

            //Shop
            modelBuilder.Entity<Shop>()
                .HasOne<User>(s => s.User)
                .WithOne(u => u.Shop)
                .HasForeignKey<Shop>(s => s.UserId);   

            base.OnModelCreating(modelBuilder);
        }
    }
}