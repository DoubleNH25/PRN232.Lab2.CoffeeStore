namespace PRN232.Lab2.CoffeeStore.Services.Models.Responses
{
    public class OrderResponseModel
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? Status { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? UserName { get; set; }
    }
}