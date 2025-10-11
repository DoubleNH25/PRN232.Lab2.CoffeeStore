namespace PRN232.Lab2.CoffeeStore.Repositories.Entities;

public class Category : BaseEntity
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ICollection<Product> Products { get; set; } = new HashSet<Product>();
}
