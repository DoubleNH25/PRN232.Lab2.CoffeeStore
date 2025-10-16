namespace PRN232.Lab2.CoffeeStore.Services.Models.Responses
{
    public class ProductResponseModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? CategoryId { get; set; }
        public bool? IsActive { get; set; }
        public string? CategoryName { get; set; }
    }
}