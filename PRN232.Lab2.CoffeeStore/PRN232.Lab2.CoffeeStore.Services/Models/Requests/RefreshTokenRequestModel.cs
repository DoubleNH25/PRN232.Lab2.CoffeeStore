namespace PRN232.Lab2.CoffeeStore.Services.Models.Requests;

public class RefreshTokenRequestModel
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
