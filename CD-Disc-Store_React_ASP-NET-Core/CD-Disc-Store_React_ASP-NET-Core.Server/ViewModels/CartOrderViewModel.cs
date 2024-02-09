namespace CD_Disc_Store_React_ASP_NET_Core.Server.ViewModels
{
    public class CartOrderViewModel
    {
        public List<OrderItem> OrderItems { get; set; } = new();
        public List<Order> Orders { get; set; } = new();
        public List<OperationLog> OperationLogs { get; set; } = new();
    }
}
