using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.DataContext
{
    public class WebApiDbContext : DbContext
    {
        public WebApiDbContext() { }

        public WebApiDbContext(DbContextOptions<WebApiDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Loan> Loans { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Loan>()
                .HasKey(t => new { t.Id });

            modelBuilder.Entity<Loan>()
                .HasOne(pt => pt.Book)
                .WithMany(p => p.Loans)
                .HasForeignKey(pt => pt.BookId)
                .IsRequired();

            modelBuilder.Entity<Loan>()
                .HasOne(pt => pt.Reader)
                .WithMany(t => t.Loans)
                .HasForeignKey(pt => pt.ReaderId)
                .IsRequired();
        }
    }
}
