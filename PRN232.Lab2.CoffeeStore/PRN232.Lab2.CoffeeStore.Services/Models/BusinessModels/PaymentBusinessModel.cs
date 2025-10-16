namespace PRN232.Lab2.CoffeeStore.Services.Models.BusinessModels
{
    public class PaymentBusinessModel
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? PaymentMethod { get; set; }

        public virtual OrderBusinessModel Order { get; set; } = null!;
    }
}