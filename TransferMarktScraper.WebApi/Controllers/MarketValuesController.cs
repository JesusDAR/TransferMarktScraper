using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TransferMarktScraper.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Cors;

namespace TransferMarktScraper.WebApi.Controllers
{
    [ApiController]
    [Route("/api/market-values")]
    [EnableCors("CorsPolicy")]
    public class MarketValuesController : ControllerBase
    {
        private readonly IMarketValueServices _marketValueServices;

        public MarketValuesController(IMarketValueServices marketValueServices)
        {
            _marketValueServices = marketValueServices;
        }

        [HttpGet("players/{id}")]
        public async Task<IActionResult> GetMarketValueByPlayerId(string id)
        {
            return Ok(await _marketValueServices.GetByPlayerId(id));
        }

        [HttpGet("scrape/players/{id}")]
        public async Task<IActionResult> ScrapeMarketValueByPlayerId(string id)
        {
            return Ok(await _marketValueServices.ScrapeByPlayerId(id));
        }
    }
}
