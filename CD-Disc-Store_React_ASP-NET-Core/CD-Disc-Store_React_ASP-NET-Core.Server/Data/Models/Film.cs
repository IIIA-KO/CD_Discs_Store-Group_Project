using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Atributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

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
        public string Genre { get; set; } = default!;

        [StringLength(50)]
        [Required(ErrorMessage = "The \"Producer\" field is required")]
        public string Producer { get; set; } = default!;

        [StringLength(50)]
        [Required(ErrorMessage = "The \"Main Role\" field is required")]
        public string MainRole { get; set; } = default!;

        [Range(0, 18)]
        [Required(ErrorMessage = "The \"Age Limit\" field is required")]
        public int AgeLimit { get; set; }
    }
}