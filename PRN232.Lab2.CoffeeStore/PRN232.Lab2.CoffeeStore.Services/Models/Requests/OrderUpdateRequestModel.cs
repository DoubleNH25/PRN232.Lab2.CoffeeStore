using System.ComponentModel.DataAnnotations;

namespace PRN232.Lab2.CoffeeStore.Services.Models.Requests;

public class OrderUpdateRequestModel
{
    [Required]
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = string.Empty;

    [MinLength(1)]
    public List<OrderDetailUpdateRequestModel> Details { get; set; } = new();

    public PaymentUpdateRequestModel? Payment { get; set; }
}
