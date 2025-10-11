using System.ComponentModel.DataAnnotations;

namespace PRN232.Lab2.CoffeeStore.Services.Models.Requests;

public class RegisterRequestModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = string.Empty;
}
