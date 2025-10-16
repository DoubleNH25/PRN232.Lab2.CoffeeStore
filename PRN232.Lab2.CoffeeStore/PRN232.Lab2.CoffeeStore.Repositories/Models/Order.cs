using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PRN232.Lab2.CoffeeStore.Repositories.Models
{
    [Table("Order")]
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Payments = new HashSet<Payment>();
        }

        [Key]
        public int OrderId { get; set; }
        public int UserId { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? OrderDate { get; set; }
        [StringLength(20)]
        public string? Status { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TotalAmount { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Orders")]
        public virtual User User { get; set; } = null!;
        [InverseProperty("Order")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        [InverseProperty("Order")]
        public virtual ICollection<Payment> Payments { get; set; }
    }
}