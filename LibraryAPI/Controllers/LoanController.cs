using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LibraryAPI.Data;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/reader/{readerId}/[controller]")]
    public class LoanController : ControllerBase
    {
        private readonly ILogger<LoanController> _logger;

        private readonly ILibraryRepository _libraryRepository;
   
        public LoanController(ILogger<LoanController> logger, ILibraryRepository libraryRepository)
        {
            _logger = logger;
            _libraryRepository = libraryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetLoans()
        {
            var books = await _libraryRepository.GetLoans();
            return Ok(books);
        }
    }
}
