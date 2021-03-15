using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryWebApp.Dtos
{
    public class LoanForRegisterDto
    {
        public int ReaderId { get; set; }
        public int BookId { get; set; } 
    }
}
