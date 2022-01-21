using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransferMarktScraper.Core.Entities;
using TransferMarktScraper.WebApi.DTOs;

namespace TransferMarktScraper.WebApi.Services.Interfaces
{
    public interface IPlayerServices
    {
        Task<Player> Get(string id);
        Task<IEnumerable<Player>> GetAll();
        Task<IEnumerable<Player>> GetAllByTeamId(string id);
        Task<Player> Add(Player player);
        Task<Team> AddAllToTeam(Team team, IEnumerable<Player> players);
        Task Update(FilterDefinition<Player> filter, UpdateDefinition<Player> update);
        Task DeleteAll();
        Task DeleteAllByTeamId(string id);
        Task<ScrapeResults> ScrapeByTeamId(string id);      
    }
}
