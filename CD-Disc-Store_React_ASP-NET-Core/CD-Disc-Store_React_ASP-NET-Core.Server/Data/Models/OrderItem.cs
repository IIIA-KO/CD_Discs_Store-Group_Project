using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models
{
    [Table("OrderItem")]
    public class OrderItem
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "The \"Id Order\" field is required")]
        public Guid IdOrder { get; set; }

        [Required(ErrorMessage = "The \"Id Disc\" field is required")]
        public Guid IdDisc { get; set; }

        [Range(1, int.MaxValue)]
        [Required(ErrorMessage = "The \"Quantity\" field is required")]
        public int Quantity { get; set; }
    }
}
