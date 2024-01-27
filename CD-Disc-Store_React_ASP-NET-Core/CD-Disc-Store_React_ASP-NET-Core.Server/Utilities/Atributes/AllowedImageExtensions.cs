using System.ComponentModel.DataAnnotations;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Atributes
{
    public class AllowedImageExtensions : ValidationAttribute
    {
        public readonly static string[] AllowedExtensions = [".jpg", ".jpeg", ".png"];

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!AllowedExtensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"This image file extension is not allowed.";
        }
    }
}
