namespace PRN232.Lab2.CoffeeStore.Repositories.Entities;

public class Payment : BaseEntity
{
    public int PaymentId { get; set; }
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public Order Order { get; set; } = null!;
}
