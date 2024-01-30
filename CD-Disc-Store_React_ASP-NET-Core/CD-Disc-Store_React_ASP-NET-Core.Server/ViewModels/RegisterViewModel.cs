using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Atributes;
using System.ComponentModel.DataAnnotations;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.ViewModels
{
    public class RegisterViewModel
    {
        [StringLength(50)]
        [Required(ErrorMessage = "The \"User Name\" field is required")]
        public string UserName { get; set; } = default!;

        [StringLength(100)]
        [EmailAddressValidation]
        [Required(ErrorMessage = "The \"Email\" field is required")]
        public string Email { get; set; } = default!;

        [MinLength(8)]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "The \"Password\" field is required")]
        public string Password { get; set; } = default!;

        [MinLength(8)]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "The \"Confirm Password\" field is required")]
        public string ConfirmPassword { get; set; } = default!;
    }
}
