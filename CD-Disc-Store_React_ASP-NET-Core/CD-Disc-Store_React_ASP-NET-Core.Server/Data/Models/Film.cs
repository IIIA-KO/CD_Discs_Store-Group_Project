using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Atributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models
{
    [Table("Disc")]
    public class Disc
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "The \"Name\" field is required")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "The \"Price\" field is required")]
        public decimal Price { get; set; } = default!;

        [Range(0, int.MaxValue)]
        [Required(ErrorMessage = "The \"Left On Stock\" field is required")]
        public int LeftOnStock { get; set; } = default!;

        [Range(0, 5)]
        [Required(ErrorMessage = "The \"Rating\" field is required")]
        public decimal Rating { get; set; } = default!;

        [StringLength(250)]
        public string? CoverImagePath { get; set; }

        public string? ImageStorageName { get; set; }
        
        [MaxFileSize(1 * 1024 * 1024)]
        [AllowedFileExtensions(new string[] { ".jpg", ".png", ".jpeg"})]
        public IFormFile? ImageFile { get; set; }
    }
}