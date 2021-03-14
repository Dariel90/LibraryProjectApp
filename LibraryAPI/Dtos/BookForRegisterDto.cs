using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Dtos
{
    public class BookForRegisterDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(8, ErrorMessage = "You must specify the ISBN number")]
        public string Isbn{ get; set; }

        public bool IsBorrowed { get; set; }


    }
}
