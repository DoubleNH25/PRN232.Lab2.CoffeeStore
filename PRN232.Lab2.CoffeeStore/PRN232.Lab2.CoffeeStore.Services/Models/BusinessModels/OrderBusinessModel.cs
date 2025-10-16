namespace PRN232.Lab2.CoffeeStore.Services.Models.BusinessModels
{
    public class OrderBusinessModel
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? Status { get; set; }
        public decimal? TotalAmount { get; set; }

        public virtual UserBusinessModel User { get; set; } = null!;
        public virtual ICollection<OrderDetailBusinessModel> OrderDetails { get; set; } = new List<OrderDetailBusinessModel>();
        public virtual ICollection<PaymentBusinessModel> Payments { get; set; } = new List<PaymentBusinessModel>();
    }
}