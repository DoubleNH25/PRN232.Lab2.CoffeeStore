using System.ComponentModel.DataAnnotations;

namespace PRN232.Lab2.CoffeeStore.Services.Models.Requests;

public class ProductUpdateRequestModel
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    public bool IsActive { get; set; }

    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }
}
