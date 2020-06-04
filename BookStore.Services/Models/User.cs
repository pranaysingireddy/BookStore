﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Services.Models
{
    public partial class User
    {
        public User()
        {
            RefreshToken = new HashSet<RefreshToken>();
        }

        [Key]
        [Column("user_id")]
        public int UserId { get; set; }
        [Required]
        [Column("email_address")]
        [StringLength(100)]
        public string EmailAddress { get; set; }
        [Required]
        [Column("password")]
        [StringLength(100)]
        public string Password { get; set; }
        [Required]
        [Column("source")]
        [StringLength(100)]
        public string Source { get; set; }
        [Column("first_name")]
        [StringLength(20)]
        public string FirstName { get; set; }
        [Column("middle_name")]
        [StringLength(1)]
        public string MiddleName { get; set; }
        [Column("last_name")]
        [StringLength(30)]
        public string LastName { get; set; }
        [Column("role_id")]
        public short RoleId { get; set; }
        [Column("pub_id")]
        public int PubId { get; set; }
        [Column("hire_date", TypeName = "datetime")]
        public DateTime? HireDate { get; set; }

        [ForeignKey(nameof(PubId))]
        [InverseProperty(nameof(Publisher.User))]
        public virtual Publisher Pub { get; set; }
        [ForeignKey(nameof(RoleId))]
        [InverseProperty("User")]
        public virtual Role Role { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<RefreshToken> RefreshToken { get; set; }
    }
}
