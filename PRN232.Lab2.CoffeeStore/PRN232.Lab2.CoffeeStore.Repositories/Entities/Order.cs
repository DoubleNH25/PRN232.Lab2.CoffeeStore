namespace PRN232.Lab2.CoffeeStore.Repositories.Entities;

public class Order : BaseEntity
{
    public int OrderId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public int? PaymentId { get; set; }
    public Payment? Payment { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; } = new HashSet<OrderDetail>();
}
