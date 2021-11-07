using Microsoft.EntityFrameworkCore;
namespace BurgerShop.Models {
    public class DataContext: DbContext {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}
        public DbSet<Cart> Carts {get; set;}
        public DbSet<CartLine> CartLines {get; set;}
        public DbSet<Menu> Menus {get; set;}
        public DbSet<Order> Orders {get; set;}
        public DbSet<OrderLine> OrderLines {get; set;}
    }
}
