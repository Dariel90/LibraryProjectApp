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
    }
}
