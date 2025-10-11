namespace PRN232.Lab2.CoffeeStore.Services.Models.Responses;

public class OrderResponseModel
{
    public int OrderId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public int? PaymentId { get; set; }
    public IEnumerable<OrderDetailResponseModel> Details { get; set; } = Enumerable.Empty<OrderDetailResponseModel>();
    public PaymentResponseModel? Payment { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
