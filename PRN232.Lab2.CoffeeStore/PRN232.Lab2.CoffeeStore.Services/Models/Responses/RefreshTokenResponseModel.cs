namespace PRN232.Lab2.CoffeeStore.Services.Models.Responses
{
    public class RefreshTokenResponseModel
    {
        public int RefreshTokenId { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; } = null!;
        public DateTime Expires { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Revoked { get; set; }
    }
}