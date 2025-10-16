using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PRN232.Lab2.CoffeeStore.Repositories.Models
{
    [Table("RefreshToken")]
    public partial class RefreshToken
    {
        [Key]
        public int RefreshTokenId { get; set; }
        public int UserId { get; set; }
        [StringLength(500)]
        public string Token { get; set; } = null!;
        [Column(TypeName = "datetime2")]
        public DateTime Expires { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? Created { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? Revoked { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("RefreshTokens")]
        public virtual User User { get; set; } = null!;
    }
}