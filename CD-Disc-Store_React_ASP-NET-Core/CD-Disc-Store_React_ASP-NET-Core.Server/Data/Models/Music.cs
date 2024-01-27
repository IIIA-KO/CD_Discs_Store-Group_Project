using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Interfaces;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Atributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models
{
    [Table("Music")]
    public class Music : IImage
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
        [Required(ErrorMessage = "The \"Artist\" field is required")]
        public string Artist { get; set; } = default!;

        [StringLength(50)]
        [Required(ErrorMessage = "The \"Language\" field is required")]
        public string Language { get; set; } = default!;

        [StringLength(250)]
        public string? CoverImagePath { get; set; }

        [StringLength(255)]
        public string? ImageStorageName { get; set; }

        [AllowedImageExtensions]
        [MaxFileSize(1 * 1024 * 1024)]
        public IFormFile? ImageFile { get; set; }
    }
}
