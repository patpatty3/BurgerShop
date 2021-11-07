using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BurgerShop.Models {
    public class CartBindingTarget {
        [Required]
        public ICollection<CartLine> CartLines { get; set; }

        public Cart ToCart() => new Cart() {
            CartLines = this.CartLines
        };
    }
}
