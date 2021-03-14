using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace LibraryWebApp.Helper
{
    public class LibraryApiHelper
    {
        private IConfiguration _configuration { get; }
        public LibraryApiHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public HttpClient Initial()
        {
            var client = new HttpClient();
            var apiUri = _configuration.GetSection("ApiConfigSection").GetSection("ApiUri").Value;
            client.BaseAddress = new Uri(apiUri);
            return client;
        }
    }
}
