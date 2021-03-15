using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LibraryAPI.Data;
using LibraryAPI.Dtos;
using LibraryAPI.Errors;
using LibraryAPI.Models;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/book/{bookId}/[controller]")]
    public class LoanController : ControllerBase
    {
        private readonly ILogger<LoanController> _logger;

        private readonly ILibraryRepository _libraryRepository;

        private readonly IMapper _mapper;

        public LoanController(ILogger<LoanController> logger, ILibraryRepository libraryRepository, IMapper mapper)
        {
            _logger = logger;
            _libraryRepository = libraryRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetLoan")]
        public async Task<IActionResult> GetLoan(int bookId, int id)
        {
            var sender = await _libraryRepository.GetBook(bookId);
            if (sender.Id != bookId) return Unauthorized();

            var messageFromRepo = await _libraryRepository.GetLoan(id);

            if (messageFromRepo == null) return NotFound();

            return Ok(messageFromRepo);
        }

        [HttpGet("GetLoans")]
        public async Task<IActionResult> GetLoans()
        {
            var books = await _libraryRepository.GetLoans();
            return Ok(books);
        }

        [HttpGet("GetBorrowedBooksToAUser")]
        public async Task<IActionResult> GetBorrowedBooksToAUser(int readerId)
        {
            var booksFromRepo = await _libraryRepository.GetBorrowedBooksToAUser(readerId);
            return Ok(booksFromRepo);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLoan(int bookId, LoanForCreationDto loanForCreationDto)
        {
            var book = await _libraryRepository.GetBook(bookId);
            if (book == null) return NotFound("The reader was not found");

            loanForCreationDto.BookId = bookId;

            var loan = _mapper.Map<Loan>(loanForCreationDto);

            var isBookBorrowed = await _libraryRepository.IsABorrowedBook(loan.BookId);

            if (!isBookBorrowed)
            {
                book.IsBorrowed = true;
                _libraryRepository.Add(loan);

                if (await _libraryRepository.SaveAll())
                {
                    var loanToReturn = _mapper.Map<LoanToReturnDto>(loan);
                    return CreatedAtRoute("GetLoan", new { bookId, id = loan.Id }, loanToReturn);
                }
            }
            else
            {
                string message = string.Format($"Sorry, the book named {book.Name} is already on loan");
                return BadRequest(message);
                //throw new Exception(message);
            }

            return BadRequest("Creating the loan failed on save");
        }
    }
}
