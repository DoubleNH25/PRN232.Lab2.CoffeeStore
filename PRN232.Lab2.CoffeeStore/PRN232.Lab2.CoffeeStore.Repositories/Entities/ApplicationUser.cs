using Microsoft.AspNetCore.Identity;

namespace PRN232.Lab2.CoffeeStore.Repositories.Entities;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
