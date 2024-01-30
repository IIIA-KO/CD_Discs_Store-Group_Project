using System.ComponentModel.DataAnnotations;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Atributes;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.ViewModels.Account
{
    public class RegisterViewModel
    {
        [StringLength(50)]
        [Required(ErrorMessage = "The \"User Name\" field is required")]
        public string UserName { get; set; } = default!;

        [StringLength(100)]
        [EmailAddress]
        [Required(ErrorMessage = "The \"Email\" field is required")]
        public string Email { get; set; } = default!;

        [MinLength(8)]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "The \"Password\" field is required")]
        public string Password { get; set; } = default!;

        [Compare("Password")]
        [Required(ErrorMessage = "The \"Confirm password\" field is required")]
        public string ConfirmPassword { get; set; } = default!;

        [StringLength(20)]
        [Phone]
        [Required(ErrorMessage = "The \"Phone number\" field is required")]
        public string PhoneNumber { get; set; } = default!;

        [StringLength(100)]
        [Required(ErrorMessage = "The \"Address\" field is required")]
        public string Address { get; set; } = default!;

        [StringLength(50)]
        [Required(ErrorMessage = "The \"City\" field is required")]
        public string City { get; set; } = default!;

        [Required(ErrorMessage = "The \"Birth day\" field is required")]
        public DateTime BirthDay { get; set; }

        [Required(ErrorMessage = "The \"Married status\" field is required")]
        public bool MarriedStatus { get; set; } = false;

        [Required(ErrorMessage = "The \"Sex\" field is required")]
        public bool Sex { get; set; } = false;

        [Required(ErrorMessage = "The \"Has child\" field is required")]
        public bool HasChild { get; set; } = false;
    }
}
