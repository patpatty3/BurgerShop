using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BurgerShop.Models {
    public class Cart {
        public long CartId {get; set;}

        public ICollection<CartLine> CartLines {get; set;}
    }
}
