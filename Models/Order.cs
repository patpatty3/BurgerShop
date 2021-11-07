using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BurgerShop.Models {
    public class Order {
        public long OrderId {get; set;}

        public ICollection<OrderLine> OrderLines {get; set;}

        public string Address {get; set;}
    }
}
