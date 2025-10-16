namespace PRN232.Lab2.CoffeeStore.Services.Models.BusinessModels
{
    public class ProductBusinessModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? CategoryId { get; set; }
        public bool? IsActive { get; set; }

        public virtual CategoryBusinessModel? Category { get; set; }
        public virtual ICollection<OrderDetailBusinessModel> OrderDetails { get; set; } = new List<OrderDetailBusinessModel>();
    }
}