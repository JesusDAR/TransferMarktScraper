using System.Collections.Generic;
using System.Threading.Tasks;
using TransferMarktScraper.Core.Entities;
using TransferMarktScraper.WebApi.DTOs;

namespace TransferMarktScraper.WebApi.Services.Interfaces
{
    public interface IMarketValueServices
    {
        Task<MarketValue> GetByPlayerId(string id);
        Task<MarketValue> Add(MarketValue marketValue);
        Task<Player> AddToPlayer(Player player, MarketValue marketValue);
        Task DeleteByPlayerId(string id);
        Task DeleteAllByPlayerIds(IEnumerable<string> ids);
        Task<ScrapeResults> ScrapeByPlayerId(string id);
    }
}
