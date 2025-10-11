using System.ComponentModel.DataAnnotations;

namespace PRN232.Lab2.CoffeeStore.Services.Models.Requests;

public class OrderDetailUpdateRequestModel
{
    [Range(0, int.MaxValue)]
    public int OrderDetailId { get; set; }

    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal UnitPrice { get; set; }
}
