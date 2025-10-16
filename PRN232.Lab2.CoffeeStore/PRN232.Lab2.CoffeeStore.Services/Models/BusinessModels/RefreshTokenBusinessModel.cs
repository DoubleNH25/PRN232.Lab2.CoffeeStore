namespace PRN232.Lab2.CoffeeStore.Services.Models.BusinessModels
{
    public class RefreshTokenBusinessModel
    {
        public int RefreshTokenId { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; } = null!;
        public DateTime Expires { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Revoked { get; set; }

        public virtual UserBusinessModel User { get; set; } = null!;
    }
}