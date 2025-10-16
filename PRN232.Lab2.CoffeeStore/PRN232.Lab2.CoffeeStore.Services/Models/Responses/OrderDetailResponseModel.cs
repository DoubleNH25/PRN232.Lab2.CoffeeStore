namespace PRN232.Lab2.CoffeeStore.Services.Models.Responses
{
    public class OrderDetailResponseModel
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string? ProductName { get; set; }
    }
}