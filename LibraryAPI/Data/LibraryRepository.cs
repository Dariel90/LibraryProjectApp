using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.DataContext;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Data
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly WebApiDbContext _context;
        public LibraryRepository(WebApiDbContext context) => _context = context;

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Book> GetBook(int id)
        {
            return await _context.Books.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Loan> GetLoan(int id)
        {
            return await _context.Loans.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async void SetBookInLoan(int id)
        {
            var book = await GetBook(id);
            if (book.Id != id) return;
            book.IsBorrowed = true;
        }

        public async Task<List<Book>> GetBooks()
        {
            var books = new List<Book>();

            try
            {
                books = await _context.Books.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return books;
        }

        public async Task<List<Reader>> GetReaders()
        {
            var readers = new List<Reader>();

            try
            {
                readers = await _context.Readers.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return readers;
        }

        public async Task<List<Book>> GetBorrowedBooksToAUser(int id)
        {
            var books = new List<Book>();
            try
            {
                var query = _context.Loans.Include(lb => lb.Reader).Include(lr => lr.Book).Where(lr => lr.ReaderId == id);
                books = await query.Select(b => new Book
                {
                    Id = b.Book.Id,
                    Name = b.Book.Name,
                    Isbn = b.Book.Isbn,
                    IsBorrowed = b.Book.IsBorrowed
                }).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return books;
        }

        public async Task<List<Loan>> GetLoans()
        {
            var loans = new List<Loan>();

            try
            {
                loans = await _context.Loans.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return loans;
        }

        public async Task<Reader> GetReader(int id)
        {
            return await _context.Readers.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<bool> IsABookBorrowedToAUser(int bookId, int readerId) {
            
            return await _context.Loans.FirstOrDefaultAsync(l => l.BookId == bookId && l.ReaderId == readerId) != null;
        }

        public async Task<bool> IsABorrowedBook(int id)
        {
            var result = false;
            var book = await GetBook(id);
            if (book.Id == id)
            {
                result = book.IsBorrowed;
            }
            return result;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
