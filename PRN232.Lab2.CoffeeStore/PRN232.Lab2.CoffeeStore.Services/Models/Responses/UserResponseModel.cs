namespace PRN232.Lab2.CoffeeStore.Services.Models.Responses
{
    public class UserResponseModel
    {
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Role { get; set; } = null!;
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
    }
}