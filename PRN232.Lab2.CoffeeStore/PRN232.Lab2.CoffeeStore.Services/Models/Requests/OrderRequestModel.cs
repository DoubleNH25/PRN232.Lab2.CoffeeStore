using System.ComponentModel.DataAnnotations;

namespace PRN232.Lab2.CoffeeStore.Services.Models.Requests
{
    public class OrderRequestModel
    {
        [Required]
        public int UserId { get; set; }

        public DateTime? OrderDate { get; set; }

        [StringLength(20)]
        public string? Status { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Total amount must be greater than or equal to 0")]
        public decimal? TotalAmount { get; set; }
    }
}