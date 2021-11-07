using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
namespace BurgerShop.Models {
    public static class SeedData {
        public static void SeedDatabase(DataContext context) {
            context.Database.Migrate();
            if (context.Carts.Count() == 0
                && context.CartLines.Count() == 0
                && context.Menus.Count() == 0
                && context.Orders.Count() == 0
                && context.OrderLines.Count() == 0
            ) {
                Menu m1 = new Menu {Name = "Cheese Burger", Price = 89};
                Menu m2 = new Menu {Name = "Fries", Price = 49};
                Menu m3 = new Menu {Name = "Pasta", Price = 199};
                CartLine cl1 = new CartLine {Menu = m1, Quantity = 1};
                CartLine cl2 = new CartLine {Menu = m2, Quantity = 2};
                Cart c1 = new Cart {CartLines = new List<CartLine>() {cl1, cl2}};
                OrderLine ol1 = new OrderLine {Menu = m2, Quantity = 4};
                OrderLine ol2 = new OrderLine {Menu = m3, Quantity = 5};
                Order o1 = new Order {OrderLines = new List<OrderLine>() {ol1, ol2}, Address = "Bangkok"};
                context.Menus.AddRange(m1, m2, m3);
                context.Carts.AddRange(c1);
                context.Orders.AddRange(o1);
                context.SaveChanges();
            }
        }
    }
}
