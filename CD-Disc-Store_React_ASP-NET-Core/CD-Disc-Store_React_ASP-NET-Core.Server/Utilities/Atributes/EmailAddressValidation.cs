using System.ComponentModel.DataAnnotations;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Atributes
{
    public class EmailAddressValidation : RegularExpressionAttribute
    {
        public readonly static string EmailPattern = @"^([a-zA-Zа-яА-ЯіІїЇєЄ0–9._%-]+@[a-zA-Zа-яА-ЯіІїЇєЄ0–9.-]+\.[a-zA-Zа-яА-ЯіІїЇєЄ]{2,6})*$";

        public EmailAddressValidation() : base(EmailPattern)
        {
            ErrorMessage = "Invalid email format";
        }
    }
}
