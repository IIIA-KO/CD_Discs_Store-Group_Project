using System.ComponentModel.DataAnnotations;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.ViewModels.Account
{
    public class LoginViewModel
    {
        [StringLength(50)]
        [Required(ErrorMessage = "The \"User Name\" field is required")]
        public string UserName { get; set; } = default!;

        [MinLength(8)]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "The \"Password\" field is required")]
        public string Password { get; set; } = default!;
    }
}
