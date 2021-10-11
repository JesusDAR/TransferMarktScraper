using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransferMarktScraper.Core.Models;
using TransferMarktScraper.WebApi.DTOs;

namespace TransferMarktScraper.WebApi.Services.Interfaces
{
    public interface IPerformanceServices
    {
        Task<Performance> GetPerformanceByPlayerId(string id);
        Task<Performance> AddPerformance(Performance performance);
        Task<Player> AddPerformanceToPlayer(Player player, Performance performance);
        Task<ScrapeResults> ScrapePerformanceByPlayerId(string id);
    }
}
