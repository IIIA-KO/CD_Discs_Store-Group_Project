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

        [Required(ErrorMessage = "The \"LeftOnStock\" field is required")]
        public int LeftOnStock { get; set; } = default!;

        [Required(ErrorMessage = "The \"Rating\" field is required")]
        public decimal Rating { get; set; } = default!;


    }
}
