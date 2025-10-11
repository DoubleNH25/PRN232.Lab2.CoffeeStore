using System.ComponentModel.DataAnnotations;

namespace PRN232.Lab2.CoffeeStore.Services.Models.Requests;

public class OrderCreateRequestModel
{
    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = string.Empty;

    [MinLength(1)]
    public List<OrderDetailCreateRequestModel> Details { get; set; } = new();

    public PaymentCreateRequestModel? Payment { get; set; }
}
