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

        private readonly ILibraryRepository _libraryService;
   
        public LoanController(ILogger<LoanController> logger, ILibraryRepository libraryService)
        {
            _logger = logger;
            _libraryService = libraryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLoans()
        {
            var books = await _libraryService.GetLoansAsync();
            return Ok(books);
        }
    }
}
