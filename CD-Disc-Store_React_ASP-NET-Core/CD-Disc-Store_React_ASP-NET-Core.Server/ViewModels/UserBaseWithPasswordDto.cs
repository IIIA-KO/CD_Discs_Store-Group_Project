using System.ComponentModel.DataAnnotations;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.ViewModels
{
    public class UserBaseWithPasswordDto : UserBaseDto
    {
        [MinLength(8)]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "The \"Password\" field is required")]
        public string Password { get; set; } = default!;
    }
}
