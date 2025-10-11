namespace PRN232.Lab2.CoffeeStore.Repositories.Entities;

public abstract class BaseEntity
{
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
