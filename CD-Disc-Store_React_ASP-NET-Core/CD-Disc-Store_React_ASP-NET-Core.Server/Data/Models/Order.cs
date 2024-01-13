using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The \"Order Date Time\" field is required")]
        public DateTime OrderDateTime { get; set; }

        [Required(ErrorMessage = "The \"Id Client\" field is required")]
        public Guid IdClient { get; set; }
    }
}