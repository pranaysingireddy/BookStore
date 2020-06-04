using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Services.Models
{
    public partial class Role
    {
        public Role()
        {
            User = new HashSet<User>();
        }

        [Key]
        [Column("role_id")]
        public short RoleId { get; set; }
        [Required]
        [Column("role_desc")]
        [StringLength(50)]
        public string RoleDesc { get; set; }

        [InverseProperty("Role")]
        public virtual ICollection<User> User { get; set; }
    }
}
