using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransferMarktScraper.Core.Models;
using TransferMarktScraper.WebApi.DTOs;

namespace TransferMarktScraper.WebApi.Services.Interfaces
{
    public interface IPlayerServices
    {
        Task<IEnumerable<Player>> GetPlayers();
        Task<Player> GetPlayer(string id);
        Task<IEnumerable<Player>> GetPlayersByTeamId(string id);
        Task<Player> AddPlayer(Player player);
        Task<Team> AddPlayersToTeam(Team team, IEnumerable<Player> players);
        Task UpdatePlayer(FilterDefinition<Player> filter, UpdateDefinition<Player> update);
        Task DeletePlayers();
        Task DeletePlayersByTeamId(string id);
        Task<ScrapeResults> ScrapePlayersByTeamId(string id);


        
    }
}
