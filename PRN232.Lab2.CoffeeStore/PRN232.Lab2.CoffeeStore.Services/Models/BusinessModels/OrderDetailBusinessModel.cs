namespace PRN232.Lab2.CoffeeStore.Services.Models.BusinessModels
{
    public class OrderDetailBusinessModel
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public virtual OrderBusinessModel Order { get; set; } = null!;
        public virtual ProductBusinessModel Product { get; set; } = null!;
    }
}