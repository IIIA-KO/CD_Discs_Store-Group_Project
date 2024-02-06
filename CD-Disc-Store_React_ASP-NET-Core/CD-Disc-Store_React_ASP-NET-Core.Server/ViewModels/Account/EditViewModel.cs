using System.ComponentModel.DataAnnotations;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Atributes;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.ViewModels.Account
{
    public class EditViewModel
    {

        [StringLength(50)]
        [Required(ErrorMessage = "The \"User Name\" field is required")]
        public string UserName { get; set; } = default!;

        [StringLength(100)]
        [EmailAddress]
        [Required(ErrorMessage = "The \"Email\" field is required")]
        public string Email { get; set; } = default!;

        [StringLength(20)]
        [Phone]
        [Required(ErrorMessage = "The \"Phone number\" field is required")]
        public string PhoneNumber { get; set; } = default!;
    }
}
