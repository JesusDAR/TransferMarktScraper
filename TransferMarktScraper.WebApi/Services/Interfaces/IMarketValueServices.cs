using System.Collections.Generic;
using System.Threading.Tasks;
using TransferMarktScraper.Core.Models;
using TransferMarktScraper.WebApi.DTOs;

namespace TransferMarktScraper.WebApi.Services.Interfaces
{
    public interface IMarketValueServices
    {
        Task<MarketValue> GetMarketValueByPlayerId(string id);
        Task<MarketValue> AddMarketValue(MarketValue marketValue);
        Task<Player> AddMarketValueToPlayer(Player player, MarketValue marketValue);
        Task DeleteMarketValueByPlayerId(string id);
        Task DeleteMarketValuesByPlayerIds(IEnumerable<string> ids);
        Task<ScrapeResults> ScrapeMarketValueByPlayerId(string id);
    }
}
