namespace PRN232.Lab2.CoffeeStore.Services.Models.Responses;

public class AuthResponseModel
{
    public bool Succeeded { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? AccessTokenExpiration { get; set; }
}
