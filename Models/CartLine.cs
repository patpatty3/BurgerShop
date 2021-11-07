using System.ComponentModel.DataAnnotations.Schema;

namespace BurgerShop.Models {
    public class CartLine {
        public long CartLineId {get; set;}

        public long MenuId {get; set;}

        public Menu Menu {get; set;}

        public int Quantity {get; set;}
    }
}
