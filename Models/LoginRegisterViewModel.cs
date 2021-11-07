using System.ComponentModel.DataAnnotations;

namespace BurgerShop.Models {
    public class LoginRegisterViewModel {
        [Required][EmailAddress]
        public string Email {get; set;}

        [Required]
        public string Password {get; set;}
    }
}
