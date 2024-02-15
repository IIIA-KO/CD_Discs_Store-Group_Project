using System.ComponentModel.DataAnnotations;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.ViewModels
{
    public class CartDto
    {
        [Required(ErrorMessage = "The \"Id Disc\" field is required")]
        public Guid IdDisc { get; set; }

        [Range(1, int.MaxValue)]
        [Required(ErrorMessage = "The \"Quantity\" field is required")]
        public int Quantity { get; set; }
    }
}
