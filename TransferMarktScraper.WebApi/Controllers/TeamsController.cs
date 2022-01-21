using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;
using TransferMarktScraper.WebApi.DTOs;
using TransferMarktScraper.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Cors;

namespace TransferMarktScraper.WebApi.Controllers
{
    [ApiController]
    [Route("/api/teams")]
    [EnableCors("CorsPolicy")]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamServices _teamServices;

        public TeamsController(ITeamServices teamServices)
        {
            _teamServices = teamServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetTeams()
        {
            return Ok(await _teamServices.GetAll());
        }

        [HttpGet("prueba")]
        public IActionResult Prueba()
        {
            return Ok(new object { });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeam(string id)
        {
            return Ok(await _teamServices.Get(id));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTeams()
        {
            await _teamServices.DeleteAll();
            return NoContent();
        }


        [HttpPost("scrape")]
        public async Task<IActionResult> ScrapeTeams()
        {
            ScrapeResults results = await _teamServices.Scrape();
            return Ok(results);
        }
    }
}
