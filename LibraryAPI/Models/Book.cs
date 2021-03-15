using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Isbn { get; set; }
        public bool IsBorrowed { get; set; }
        public virtual ICollection<Loan> Loans { get; set; }
    }
}
