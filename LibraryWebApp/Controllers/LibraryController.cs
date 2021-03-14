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

namespace LibraryWebApp.Controllers
{
    public class LibraryController : Controller
    {
        private readonly ILogger<LibraryController> _logger;
        private readonly IConfiguration _configuration;

        private LibraryApiHelper _apiHelper;

        public LibraryController(ILogger<LibraryController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _apiHelper = new LibraryApiHelper(configuration);
        }

        public async Task<IActionResult> Index()
        {
            var books = new List<BookData>();
            var client = _apiHelper.Initial();
            var res = await client.GetAsync("api/books/GetBooks");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                books = JsonConvert.DeserializeObject<List<BookData>>(results);
            }
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
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> EditBook(BookData book)
        {
            var client = _apiHelper.Initial();
            var result = await client.PutAsJsonAsync<BookData>($"api/books/{book.Id}", book);
            if (result.IsSuccessStatusCode) Redirect("~/Index");
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
