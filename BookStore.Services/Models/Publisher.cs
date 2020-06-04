using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Services.Models
{
    public partial class Publisher
    {
        public Publisher()
        {
            Book = new HashSet<Book>();
            User = new HashSet<User>();
        }

        [Key]
        [Column("pub_id")]
        public int PubId { get; set; }
        [Column("publisher_name")]
        [StringLength(40)]
        public string PublisherName { get; set; }
        [Column("city")]
        [StringLength(20)]
        public string City { get; set; }
        [Column("state")]
        [StringLength(2)]
        public string State { get; set; }
        [Column("country")]
        [StringLength(30)]
        public string Country { get; set; }

        [InverseProperty("Pub")]
        public virtual ICollection<Book> Book { get; set; }
        [InverseProperty("Pub")]
        public virtual ICollection<User> User { get; set; }
    }
}
