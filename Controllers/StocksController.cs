using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Options;

namespace StocksApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StocksController : ControllerBase
    {
        private readonly ILogger<StocksController> _logger;
        private readonly IOptions<StocksSettings> _settings;

        public StocksController(ILogger<StocksController> logger, IOptions<StocksSettings> settings)
        {
            _logger = logger;
            _settings = settings;
        }

        [HttpGet]
        [Route("quotes/{tiker}")]
        public async Task<JsonResult> GetQuotes(string tiker)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_settings.Value.url}/market/get-quotes?region=US&symbols={tiker}"),
                Headers =
                {
                    { "x-rapidapi-key", _settings.Value.rapidapiKey },
                    { "x-rapidapi-host", _settings.Value.rapidapiHost },
                },
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
                var obj = JObject.Parse(body);
                return new JsonResult(obj);
            }

        }
    }
}
