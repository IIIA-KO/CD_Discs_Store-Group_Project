using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Atributes;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Interfaces
{
    public interface IImage
    {
        [NotNull]
        public string Name { get; set; }

        [StringLength(250)]
        public string? CoverImagePath { get; set; }

        [StringLength(255)]
        public string? ImageStorageName { get; set; }

        [AllowedImageExtensions]
        [MaxFileSize(1 * 1024 * 1024)]
        public IFormFile? ImageFile { get; set; }
    }
}
