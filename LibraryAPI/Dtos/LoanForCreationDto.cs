using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Dtos
{
    public class LoanForCreationDto
    {
        public int ReaderId { get; set; }
        public int BookId { get; set; }
    }
}
