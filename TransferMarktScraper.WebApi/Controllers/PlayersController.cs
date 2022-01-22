using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TransferMarktScraper.WebApi.DTOs;
using TransferMarktScraper.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Cors;

namespace TransferMarktScraper.WebApi.Controllers
{
    [ApiController]
    [Route("/api/players")]
    [EnableCors("CorsPolicy")]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerServices _playerServices;

        public PlayersController(IPlayerServices playerServices)
        {
            _playerServices = playerServices;
        }
        [HttpGet]
        public async Task<IActionResult> GetPlayers()
        {
            return Ok(await _playerServices.GetAll());
        }
        [HttpGet("teams/{id}")]
        public async Task<IActionResult> GetPlayersByTeamId(string id)
        {
            return Ok(await _playerServices.GetAllByTeamId(id));
        }
        [HttpDelete]
        public async Task<IActionResult> DeletePlayers()
        {
            await _playerServices.DeleteAll();
            return NoContent();
        }
        [HttpDelete("teams/{id}")]
        public async Task<IActionResult> DeletePlayersByTeamId(string id)
        {
            await _playerServices.DeleteAllByTeamId(id);
            return NoContent();
        }
        [HttpGet("scrape/teams/{id}")]
        public async Task<IActionResult> ScrapePlayersByTeamId(string id)
        {
            ScrapeResults results = await _playerServices.ScrapeByTeamId(id);
            return Ok(results);
        }
    }
}
