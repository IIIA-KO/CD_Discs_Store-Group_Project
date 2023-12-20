using System.ComponentModel.DataAnnotations;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Atributes
{
    public class PhoneValidation : RegularExpressionAttribute
    {
        public readonly static string PhonePattern = @"^(050|073|075|093|096|095|098|063|067|068)\d{7}$";
        public PhoneValidation() : base(PhonePattern)
        {
            ErrorMessage = "Invalid phone format";
        }
    }
}
