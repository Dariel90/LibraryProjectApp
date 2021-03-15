using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LibraryWebApp.Models;
using LibraryWebApp.Helper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Json;
using LibraryWebApp.Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryWebApp.Controllers
{
    public class LibraryController : Controller
    {
        private readonly ILogger<LibraryController> _logger;
        private readonly IConfiguration _configuration;

        private LibraryApiHelper _apiHelper;

        private async Task<List<BookData>> GetBooks()
        {
            var books = new List<BookData>();
            var client = _apiHelper.Initial();
            var res = await client.GetAsync("api/books/GetBooks");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                books = JsonConvert.DeserializeObject<List<BookData>>(results);
            }
            return books;
        }

        private async Task<List<ReaderData>> GetReaders()
        {
            var readers = new List<ReaderData>();
            var client = _apiHelper.Initial();
            var res = await client.GetAsync("api/reader/GetReaders");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                readers = JsonConvert.DeserializeObject<List<ReaderData>>(results);
            }
            return readers;
        }

        public LibraryController(ILogger<LibraryController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _apiHelper = new LibraryApiHelper(configuration);
        }

        public async Task<IActionResult> Index()
        {
            var books = await GetBooks();
            return View(books);
        }

        public async Task<IActionResult> Details(int id)
        {
            var book = new BookData();
            var client = _apiHelper.Initial();
            var res = await client.GetAsync($"api/books/{id}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                book = JsonConvert.DeserializeObject<BookData>(results);
            }
            return View(book);
        }

        [HttpPost]
        public IActionResult Create(BookForRegisterDto book)
        {
            var client = _apiHelper.Initial();
            var postTask = client.PostAsJsonAsync<BookForRegisterDto>("api/books/AddBook",book);
            postTask.Wait();
            var result = postTask.Result;
            if (result.IsSuccessStatusCode) return RedirectToAction("Index");
            ModelState.AddModelError(string.Empty, "Please fill the ISBN number.");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> EditBook(BookData book)
        {
            var client = _apiHelper.Initial();
            var result = await client.PutAsJsonAsync<BookData>($"api/books/{book.Id}", book);
            if (result.IsSuccessStatusCode) return RedirectToAction("Index"); ;
            return View("Edit");
        }

        public ActionResult Create()
        {
            return View();
        }

        public async Task<ActionResult> Edit(int id)
        {
            var book = new BookData();
            var client = _apiHelper.Initial();
            var res = await client.GetAsync($"api/books/{id}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                book = JsonConvert.DeserializeObject<BookData>(results);
            }
            return View(book);
        }

        public async Task<IActionResult> Delete(int id) {
            
            var client = _apiHelper.Initial();
            await client.DeleteAsync($"api/books/{id}");
            return RedirectToAction("Index");
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult CreateLoan(int id)
        {
            var readers = GetReaders().Result;
            var listItems = new List<SelectListItem>();
            foreach (var reader in readers)
            {
                listItems.Add(new SelectListItem(reader.Name, reader.Id.ToString()));
            }

            ViewData["LoanData"] = new LoanData {ReaderId = id, ReaderListItems = listItems};
            return View(new LoanData { ReaderId = id,ReaderListItems = listItems});
        }

        [HttpPost]
        public async Task<IActionResult> CreateLoan(LoanForRegisterDto loan)
        {
            var client = _apiHelper.Initial();
            var sbookId = (string)this.ControllerContext.RouteData.Values["id"];
            var bookId = Convert.ToInt32(sbookId);
            loan.BookId = bookId;
            var postTask = client.PostAsJsonAsync<LoanForRegisterDto>($"api/book/{loan.BookId}/Loan", loan);
            postTask.Wait();
            var response = postTask.Result;
            if (response.IsSuccessStatusCode) return RedirectToAction("Index");
            else
            {
                var httpErrorObject = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, httpErrorObject);
            }

            var readers = GetReaders().Result;
            var listItems = new List<SelectListItem>();
            foreach (var reader in readers)
            {
                listItems.Add(new SelectListItem(reader.Name, reader.Id.ToString()));
            }
            return View(new LoanData { ReaderId = loan.ReaderId, ReaderListItems = listItems });
        }
    }
}
