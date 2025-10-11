using System.ComponentModel.DataAnnotations;

namespace PRN232.Lab2.CoffeeStore.Services.Models.Requests;

public class PaymentUpdateRequestModel
{
    [Range(1, int.MaxValue)]
    public int PaymentId { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }

    [Required]
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(100)]
    public string PaymentMethod { get; set; } = string.Empty;
}
