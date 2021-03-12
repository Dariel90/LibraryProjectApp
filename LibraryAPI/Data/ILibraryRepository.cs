using LibraryAPI.DataContext;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryAPI.Data
{
    public interface ILibraryRepository
    {
        Task<List<Book>> GetBooks();
        Task<List<Loan>> GetLoans();
        Task<List<Book>> GetBorrowedBooksToAUser(int id);
        Task<bool> IsABookBorrowedToAUser(int bookId, int userId);
        Task<bool> IsABorrowedBook(int id);
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<Reader> GetReader(int id);
        Task<Book> GetBook(int id);

    }

}