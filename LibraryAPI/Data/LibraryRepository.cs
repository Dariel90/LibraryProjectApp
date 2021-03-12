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

        public Task<List<Book>> GetBorrowedBooksToAUser(int id)
        {
            throw new NotImplementedException();
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

        public Task<bool> IsABookBorrowedToAUser(int bookId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsABorrowedBook(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
