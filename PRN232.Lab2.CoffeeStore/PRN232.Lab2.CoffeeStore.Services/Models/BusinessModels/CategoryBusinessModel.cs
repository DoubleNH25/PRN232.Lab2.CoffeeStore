namespace PRN232.Lab2.CoffeeStore.Services.Models.BusinessModels
{
    public class CategoryBusinessModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public virtual ICollection<ProductBusinessModel> Products { get; set; } = new List<ProductBusinessModel>();
    }
}