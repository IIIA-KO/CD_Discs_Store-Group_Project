using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models
{
	[Table("OperationLog")]
	public class OperationLog
	{
		[Key]
        public Guid Id { get; set; }

		[Required(ErrorMessage = "The \"Operation Date Time Start\" field is required")]
		public DateTime OperationDateTimeStart { get; set; }

        public DateTime? OperationDateTimeEnd { get; set; }

		[Required(ErrorMessage = "The \"Client\" field is required")]
		public Guid ClientId { get; set; }

		[Required(ErrorMessage = "The \"Disc\" field is required")]
		public Guid DiscId { get; set; }

		[StringLength(50)]
		[Required(ErrorMessage = "The \"Operation Type\" field is required")]
		public string OperationType { get; set; } = null!;

        public int Quantity { get; set; }
    }
}
