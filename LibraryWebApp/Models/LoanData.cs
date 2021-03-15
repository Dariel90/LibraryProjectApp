using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryWebApp.Models
{
    public class LoanData
    {
        public List<SelectListItem> ReaderListItems { get; set; }
        public int ReaderId { get; set; }
    }
}
