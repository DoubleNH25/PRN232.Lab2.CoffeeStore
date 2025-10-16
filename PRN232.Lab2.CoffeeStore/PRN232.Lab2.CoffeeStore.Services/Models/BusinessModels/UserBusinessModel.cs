namespace PRN232.Lab2.CoffeeStore.Services.Models.BusinessModels
{
    public class UserBusinessModel
    {
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Role { get; set; } = "Staff";
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<OrderBusinessModel> Orders { get; set; } = new List<OrderBusinessModel>();
        public virtual ICollection<RefreshTokenBusinessModel> RefreshTokens { get; set; } = new List<RefreshTokenBusinessModel>();
    }
}