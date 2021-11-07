using System.ComponentModel.DataAnnotations;

namespace BurgerShop.Models {
    public class MenuBindingTarget {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public decimal Price { get; set; }

        public Menu ToMenu() => new Menu() {
            Name = this.Name,
            Price = this.Price
        };
    }
}
