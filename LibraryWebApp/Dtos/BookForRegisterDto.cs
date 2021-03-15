using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryWebApp.Dtos
{
    public class BookForRegisterDto
    {
        public string Name { get; set; }
        public string Isbn { get; set; }
        public bool IsBorrowed { get; set; }
    }
}
