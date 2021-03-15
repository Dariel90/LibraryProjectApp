using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LibraryAPI.Data;
using LibraryAPI.Dtos;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReaderController : ControllerBase
    {
        private readonly ILogger<ReaderController> _logger;

        private readonly ILibraryRepository _libraryRepository;

        private readonly IMapper _mapper;

        public ReaderController(ILogger<ReaderController> logger, ILibraryRepository libraryRepository, IMapper mapper)
        {
            _logger = logger;
            _libraryRepository = libraryRepository;
            _mapper = mapper;
        }

        [HttpGet("GetReaders")]
        public async Task<IActionResult> GetReaders()
        {
            var readers = await _libraryRepository.GetReaders();
            return Ok(readers);
        }

        [HttpGet("{id}", Name = "GetReader")]
        public async Task<IActionResult> GetReader(int id)
        {
            var reader = await _libraryRepository.GetReader(id);
            var bookToReturn = _mapper.Map<ReaderForDetailDto>(reader);
            return Ok(bookToReturn);
        }

        [HttpPost("AddReader")]
        public async Task<IActionResult> AddReader(ReaderForDetailDto readerForDetail)
        {
            var readerToCreate = _mapper.Map<Reader>(readerForDetail);
            _libraryRepository.Add(readerToCreate);

            if (await _libraryRepository.SaveAll())
            {
                var readerToReturn = _mapper.Map<ReaderForDetailDto>(readerToCreate);
                return NoContent();
            }
            return BadRequest("The process for add a reader has fail");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateReader(int id, ReaderForDetailDto readerForDetail)
        {
            var bookFromRepo = await _libraryRepository.GetReader(id);
            var updatedUser = _mapper.Map(readerForDetail, bookFromRepo);
            if (await _libraryRepository.SaveAll())
                return NoContent();
            throw new System.Exception($"Updating reader {id} failed on save");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReader(int id)
        {
            var bookFromRepo = await _libraryRepository.GetReader(id);
            _libraryRepository.Delete(bookFromRepo);
            if (await _libraryRepository.SaveAll())
                return NoContent();
            throw new System.Exception($"The process for add a book {id} has fail");
        }
    }
}
