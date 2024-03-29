using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Interfaces;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Atributes;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models
{
    [Table("Film")]
    public class Film : IImage
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

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

        [StringLength(250)]
        public string? CoverImagePath { get; set; }

        [StringLength(255)]
        public string? ImageStorageName { get; set; }

        [NotMapped]
        [AllowedImageExtensions]
        [MaxFileSize(1 * 1024 * 1024)]
        public IFormFile? ImageFile { get; set; }
    }
}
