using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models
{
    [Table("Client")]
    public class Client
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [StringLength(450)]
        [Required(ErrorMessage = "The \"User Id\" field is required")]
        public string UserId { get; set; } = default!;

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
