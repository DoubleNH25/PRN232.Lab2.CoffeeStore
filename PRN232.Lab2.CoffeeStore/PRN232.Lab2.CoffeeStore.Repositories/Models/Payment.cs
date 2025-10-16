using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PRN232.Lab2.CoffeeStore.Repositories.Models
{
    [Table("Payment")]
    public partial class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? PaymentDate { get; set; }
        [StringLength(50)]
        public string? PaymentMethod { get; set; }

        [ForeignKey("OrderId")]
        [InverseProperty("Payments")]
        public virtual Order Order { get; set; } = null!;
    }
}