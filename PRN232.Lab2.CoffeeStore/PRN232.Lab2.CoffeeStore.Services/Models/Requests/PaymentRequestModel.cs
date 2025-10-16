using System.ComponentModel.DataAnnotations;

namespace PRN232.Lab2.CoffeeStore.Services.Models.Requests
{
    public class PaymentRequestModel
    {
        [Required]
        public int OrderId { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        public DateTime? PaymentDate { get; set; }

        [StringLength(50)]
        public string? PaymentMethod { get; set; }
    }
}