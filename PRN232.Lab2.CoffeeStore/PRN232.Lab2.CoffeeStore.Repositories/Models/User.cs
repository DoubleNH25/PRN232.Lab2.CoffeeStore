using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PRN232.Lab2.CoffeeStore.Repositories.Models
{
    [Table("User")]
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
            RefreshTokens = new HashSet<RefreshToken>();
        }

        [Key]
        public int UserId { get; set; }
        [StringLength(100)]
        public string Email { get; set; } = null!;
        [StringLength(256)]
        public string PasswordHash { get; set; } = null!;
        [StringLength(50)]
        public string? FirstName { get; set; }
        [StringLength(50)]
        public string? LastName { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        [Column("Role")]
        public string Role { get; set; } = "Staff";
        [Column(TypeName = "datetime2")]
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Order> Orders { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}