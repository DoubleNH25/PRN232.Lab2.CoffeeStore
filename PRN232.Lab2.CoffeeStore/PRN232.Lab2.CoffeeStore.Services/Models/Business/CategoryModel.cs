namespace PRN232.Lab2.CoffeeStore.Services.Models.Business;

public class CategoryModel
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
