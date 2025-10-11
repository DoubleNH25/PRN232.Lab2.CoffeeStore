namespace PRN232.Lab2.CoffeeStore.Services.Models.Business;

public class OrderModel
{
    public int OrderId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public int? PaymentId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public List<OrderDetailModel> Details { get; set; } = new();
}
