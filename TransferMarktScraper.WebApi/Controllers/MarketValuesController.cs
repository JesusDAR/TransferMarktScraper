﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TransferMarktScraper.Core.Models;
using TransferMarktScraper.WebApi.Services.Interfaces;

namespace TransferMarktScraper.WebApi.Controllers
{
    [ApiController]
    [Route("/api/market-values")]
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
            return Ok(await _marketValueServices.GetMarketValueByPlayerId(id));
        }

        [HttpPost("scrape/players/{id}")]
        public async Task<IActionResult> ScrapeMarketValueByPlayerId(string id)
        {
            return Ok(await _marketValueServices.ScrapeMarketValueByPlayerId(id));
        }
    }
}