using System.ComponentModel.DataAnnotations;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Atributes
{
    public class AllowedFileExtensions : ValidationAttribute
    {
        private readonly string[] _allowedExtensions;

        public AllowedFileExtensions(string[] allowedExtensions)
        {
            this._allowedExtensions = allowedExtensions;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (!(value is not IFormFile file))
            {
                var extension = Path.GetExtension(file.FileName);
                if (!this._allowedExtensions.Contains(extension.ToLower()))
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