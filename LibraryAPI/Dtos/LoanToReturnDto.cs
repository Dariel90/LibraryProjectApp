using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Dtos
{
    public class LoanToReturnDto
    {
        public string BookName { get; set; }
        public string ReaderName { get; set; }
        public DateTime LoanDate { get; set; }
    }
}
