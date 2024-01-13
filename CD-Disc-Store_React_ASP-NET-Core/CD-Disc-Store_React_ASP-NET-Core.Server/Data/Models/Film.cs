using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models
{
    [Table("Film")]
    public class Film
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "The \"Name\" field is required")]
        public string Name { get; set; } = default!;

        [StringLength(50)]
        [Required(ErrorMessage = "The \"Genre\" field is required")]
        public decimal Genre { get; set; } = default!;

        [StringLength(50)]
        [Required(ErrorMessage = "The \"Producer\" field is required")]
        public int Producer { get; set; } = default!;

        [StringLength(50)]
        [Required(ErrorMessage = "The \"MainRole\" field is required")]
        public decimal MainRole { get; set; } = default!;

        [Required(ErrorMessage = "The \"AgeLimit\" field is required")]
        public decimal AgeLimit { get; set; } = default!;

    }
}
