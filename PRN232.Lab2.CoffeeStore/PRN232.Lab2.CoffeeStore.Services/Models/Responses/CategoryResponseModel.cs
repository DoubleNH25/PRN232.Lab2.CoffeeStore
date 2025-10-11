namespace PRN232.Lab2.CoffeeStore.Services.Models.Responses;

public class CategoryResponseModel
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
