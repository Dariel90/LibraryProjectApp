using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Dtos
{
    public class BookForDetailDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Isbn { get; set; }
        public bool IsBorrowed { get; set; }
    }
}
