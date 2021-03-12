using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.DataContext.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var _context = new WebApiDbContext(serviceProvider.GetRequiredService<DbContextOptions<WebApiDbContext>>()))
            {
                // Agregando Readers a la BD
                if (_context.Readers.Any())
                {
                    return;
                }

                _context.Readers.AddRange(
                    new Reader { Name = "Dariel Amores Fernández" },
                    new Reader { Name = "Dione López Díaz" },
                    new Reader { Name = "Daniela Amores López" }
                 );

                _context.SaveChanges();


                // Agregando Books a la BD
                if (_context.Books.Any())
                {
                    return;
                }

                _context.Books.AddRange(
                    new Book
                    {
                        Name = "El viejo y el mar",
                        ISBN = $"0-8760-4565-4",
                        IsBorrowed = false,
                    },

                    new Book
                    {
                        Name = "Viaje al Centro de la Tierra",
                        ISBN = $"0-4443-8223-2",
                        IsBorrowed = true,
                    },

                    new Book
                    {
                        Name = "Canción de hielo y fuego",
                        ISBN = $"0-4694-7756-3",
                        IsBorrowed = false
                    },

                    new Book
                    {
                        Name = "The Hobbit",
                        ISBN = $"0-9788-7440-4",
                        IsBorrowed = true,
                    }
                );

                _context.SaveChanges();
            }
        }
    }
}