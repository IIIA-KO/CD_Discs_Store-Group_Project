using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Atributes;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models
{
    [Table("Client")]
    public class Client
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "The \"First name\" field is required")]
        public string FirstName { get; set; } = default!;

        [StringLength(50)]
        [Required(ErrorMessage = "The \"Last name\" field is required")]
        public string LastName { get; set; } = default!;

        [StringLength(100)]
        [Required(ErrorMessage = "The \"Address\" field is required")]
        public string Address { get; set; } = default!;

        [StringLength(50)]
        [Required(ErrorMessage = "The \"City\" field is required")]
        public string City { get; set; } = default!;

        [StringLength(20)]
        [Required(ErrorMessage = "The \"Contact phone\" field is required")]
        [PhoneValidation]
        public string ContactPhone { get; set; } = default!;

        [StringLength(100)]
        [Required(ErrorMessage = "The \"Contact mail\" field is required")]
        [EmailAddressValidation]
        public string ContactMail { get; set; } = default!;

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
