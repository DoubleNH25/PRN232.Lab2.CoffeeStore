namespace PRN232.Lab2.CoffeeStore.Repositories.Entities;

public class RefreshToken : BaseEntity
{
    public int RefreshTokenId { get; set; }
    public string Token { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }
    public bool IsRevoked { get; set; }
}
