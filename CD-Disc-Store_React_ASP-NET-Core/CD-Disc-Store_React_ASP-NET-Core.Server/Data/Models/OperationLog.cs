using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models
{
	[Table("OperationLog")]
	public class OperationLog
	{
		[Key]
        public Guid Id { get; set; } = Guid.NewGuid();

		[Required(ErrorMessage = "The \"Operation Type\" field is required")]
		public Guid OperationType { get; set; }

		[Required(ErrorMessage = "The \"Id Order\" field is required")]
        public Guid IdOrder { get; set; }
    }
}
