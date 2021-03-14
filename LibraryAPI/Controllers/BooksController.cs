using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LibraryAPI.Data;
using LibraryAPI.Dtos;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;

        private readonly ILibraryRepository _libraryRepository;

        private readonly IMapper _mapper;

        public BooksController(ILogger<BooksController> logger, ILibraryRepository libraryRepository, IMapper mapper)
        {
            _logger = logger;
            _libraryRepository = libraryRepository;
            _mapper = mapper;
        }

        [HttpGet("GetBooks")]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _libraryRepository.GetBooks();
            return Ok(books);
        }

        [HttpGet("{id}", Name = "GetBook")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await _libraryRepository.GetBook(id);
            var bookToReturn = _mapper.Map<BookForDetailDto>(book);
            return Ok(bookToReturn);
        }

        [HttpPost("AddBook")]
        public async Task<IActionResult> AddBook(BookForRegisterDto bookForRegisterDto)
        {
            var bookToCreate = _mapper.Map<Book>(bookForRegisterDto);
            _libraryRepository.Add(bookToCreate);

            if (await _libraryRepository.SaveAll())
            {
                var bookToReturn = _mapper.Map<BookForDetailDto>(bookToCreate);
                return NoContent();
            }
            return BadRequest("The process for add a book has fail");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBook(int id, BookForUpdateDto bookForUpdateDto)
        {
            var bookFromRepo = await _libraryRepository.GetBook(id);
            var updatedUser = _mapper.Map(bookForUpdateDto, bookFromRepo);
            if (await _libraryRepository.SaveAll())
                return NoContent();
            throw new System.Exception($"Updating book {id} failed on save");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            var bookFromRepo = await _libraryRepository.GetBook(id);
            _libraryRepository.Delete(bookFromRepo);
            if (await _libraryRepository.SaveAll())
                return NoContent();
            throw new System.Exception($"Updating book {id} failed on save");
        }
    }
}
