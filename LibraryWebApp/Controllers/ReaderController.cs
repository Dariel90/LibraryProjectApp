using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using LibraryWebApp.Dtos;
using LibraryWebApp.Helper;
using LibraryWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LibraryWebApp.Controllers
{
    public class ReaderController : Controller
    {
        private LibraryApiHelper _apiHelper;
        private readonly ILogger<LibraryController> _logger;
        private readonly IConfiguration _configuration;

        public ReaderController(ILogger<LibraryController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _apiHelper = new LibraryApiHelper(configuration);
        }

        private async Task<List<ReaderData>> GetReaders()
        {
            var books = new List<ReaderData>();
            var client = _apiHelper.Initial();
            var res = await client.GetAsync("api/reader/GetReaders");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                books = JsonConvert.DeserializeObject<List<ReaderData>>(results);
            }
            return books;
        }
        public async Task<IActionResult> Index()
        {
            var readers = await GetReaders();
            return View(readers);
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {

            var client = _apiHelper.Initial();
            await client.DeleteAsync($"api/reader/{id}");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Create(ReaderForRegisterDto book)
        {
            var client = _apiHelper.Initial();
            var postTask = client.PostAsJsonAsync<ReaderForRegisterDto>("api/reader/AddReader", book);
            postTask.Wait();
            var result = postTask.Result;
            if (result.IsSuccessStatusCode) RedirectToAction("Index");
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View();
        }

        public async Task<ActionResult> Edit(int id)
        {
            var reader = new ReaderData();
            var client = _apiHelper.Initial();
            var res = await client.GetAsync($"api/reader/{id}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                reader = JsonConvert.DeserializeObject<ReaderData>(results);
            }
            return View(reader);
        }

        [HttpPost]
        public async Task<ActionResult> EditReader(ReaderData reader)
        {
            var client = _apiHelper.Initial();
            var result = await client.PutAsJsonAsync<ReaderData>($"api/reader/{reader.Id}", reader);
            if (result.IsSuccessStatusCode) return RedirectToAction("Index");
            return View("Edit");
        }

        public async Task<IActionResult> Details(int id)
        {
            var reader = new ReaderData();
            var client = _apiHelper.Initial();
            var res = await client.GetAsync($"api/reader/{id}");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                reader = JsonConvert.DeserializeObject<ReaderData>(results);
            }
            return View(reader);
        }

        
    }
}