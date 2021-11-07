using System.ComponentModel.DataAnnotations.Schema;

namespace BurgerShop.Models {
    public class Menu {
        public long MenuId {get; set;}

        public string Name {get; set;}

        [Column(TypeName = "decimal(8, 2)")]
        public decimal Price {get; set;}
    }
}
