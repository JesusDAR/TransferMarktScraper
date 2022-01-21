using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TransferMarktScraper.WebApi.DTOs;
using TransferMarktScraper.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Cors;

namespace TransferMarktScraper.WebApi.Controllers
{
    [ApiController]
    [Route("/api/performances")]
    [EnableCors("CorsPolicy")]
    public class PerformancesController : ControllerBase
    {
        private readonly IPerformanceServices _performanceServices;

        public PerformancesController(IPerformanceServices performanceServices)
        {
            _performanceServices = performanceServices;
        }

        [HttpGet("players/{id}")]
        public async Task<IActionResult> GetPerformanceByPlayerId(string id)
        {
            return Ok(await _performanceServices.GetByPlayerId(id));
        }

        [HttpPost("scrape/players/{id}")]
        public async Task<IActionResult> ScrapePerformanceByPlayerId(string id)
        {
            ScrapeResults results = await _performanceServices.ScrapePlayerId(id);
            return Ok(results);
        }
    }
}
