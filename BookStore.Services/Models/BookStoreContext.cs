using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BookStore.Services.Models
{
    public partial class BookStoreContext : DbContext
    {
        public BookStoreContext()
        {
        }

        public BookStoreContext(DbContextOptions<BookStoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Author { get; set; }
        public virtual DbSet<Book> Book { get; set; }
        public virtual DbSet<BookAuthor> BookAuthor { get; set; }
        public virtual DbSet<Job> Job { get; set; }
        public virtual DbSet<Publisher> Publisher { get; set; }
        public virtual DbSet<RefreshToken> RefreshToken { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Sale> Sale { get; set; }
        public virtual DbSet<Store> Store { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=BookStoresDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.City).IsUnicode(false);

                entity.Property(e => e.EmailAddress).IsUnicode(false);

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.Property(e => e.Phone)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('UNKNOWN')");

                entity.Property(e => e.State)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Zip)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(e => e.Notes).IsUnicode(false);

                entity.Property(e => e.PublishedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Title).IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('UNDECIDED')");

                entity.HasOne(d => d.Pub)
                    .WithMany(p => p.Book)
                    .HasForeignKey(d => d.PubId)
                    .HasConstraintName("FK__Book__pub_id__3D5E1FD2");
            });

            modelBuilder.Entity<BookAuthor>(entity =>
            {
                entity.HasKey(e => new { e.AuthorId, e.BookId });

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.BookAuthor)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("FK__BookAutho__autho__3E52440B");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BookAuthor)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK__BookAutho__book___3F466844");
            });

            modelBuilder.Entity<Job>(entity =>
            {
                entity.Property(e => e.JobDesc)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('New Position - title not formalized yet')");
            });

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.HasKey(e => e.PubId)
                    .HasName("PK__Publishe__2515F222130EF400");

                entity.Property(e => e.City).IsUnicode(false);

                entity.Property(e => e.Country)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('USA')");

                entity.Property(e => e.PublisherName).IsUnicode(false);

                entity.Property(e => e.State)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.Property(e => e.Token).IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RefreshToken)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__RefreshTo__user___403A8C7D");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleDesc)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('New Position - title not formalized yet')");
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.Property(e => e.OrderNum).IsUnicode(false);

                entity.Property(e => e.PayTerms).IsUnicode(false);

                entity.Property(e => e.StoreId)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Sale)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK__Sale__book_id__412EB0B6");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Sale)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK__Sale__store_id__4222D4EF");
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.Property(e => e.StoreId)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.City).IsUnicode(false);

                entity.Property(e => e.State)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.StoreAddress).IsUnicode(false);

                entity.Property(e => e.StoreName).IsUnicode(false);

                entity.Property(e => e.Zip)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK_user_id_2")
                    .IsClustered(false);

                entity.Property(e => e.EmailAddress).IsUnicode(false);

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.HireDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.PubId).HasDefaultValueSql("((1))");

                entity.Property(e => e.RoleId).HasDefaultValueSql("((1))");

                entity.Property(e => e.Source).IsUnicode(false);

                entity.HasOne(d => d.Pub)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.PubId)
                    .HasConstraintName("FK__User__pub_id__440B1D61");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__User__role_id__4316F928");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
