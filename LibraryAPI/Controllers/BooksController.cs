using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;

        private readonly ILibraryRepository _libraryService;

        public BooksController(ILogger<BooksController> logger, ILibraryRepository libraryService)
        {
            _logger = logger;
            _libraryService = libraryService;
        }

        [HttpGet("GetBooks")]
        public async Task<IActionResult> Get()
        {
            var books = await _libraryService.GetBooksAsync();
            return Ok(books);
        }
    }
}
