using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
        public int ReaderId { get; set; }
        public virtual Reader Reader { get; set; }

        public DateTime LoanDate { get; set; }
    }
}
