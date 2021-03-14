using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryWebApp.Models
{
    public class BookData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayName("ISBN")]
        public string Isbn { get; set; }
        [DisplayName("Borrowed?")]
        public bool IsBorrowed { get; set; }
    }
}
