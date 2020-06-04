using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Services.Models
{
    public partial class RefreshToken
    {
        [Key]
        [Column("token_id")]
        public int TokenId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Required]
        [Column("token")]
        [StringLength(200)]
        public string Token { get; set; }
        [Column("expiry_date", TypeName = "datetime")]
        public DateTime ExpiryDate { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("RefreshToken")]
        public virtual User User { get; set; }
    }
}
