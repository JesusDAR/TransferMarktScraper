using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransferMarktScraper.Core.Entities;
using TransferMarktScraper.WebApi.DTOs;

namespace TransferMarktScraper.WebApi.Services.Interfaces
{
    public interface IPerformanceServices
    {
        Task<Performance> GetByPlayerId(string id);
        Task<Performance> Add(Performance performance);
        Task<Player> AddToPlayer(Player player, Performance performance);
        Task<ScrapeResults> ScrapePlayerId(string id);
        Task DeleteAll();
    }
}
