using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryShop.Models
{
    public class Register
    {
		[Required(ErrorMessage = "Please Enter Email Address")]
		[DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password did nit match")]
        public string ConfirmPassword { get; set; }
    }
}
