using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransferMarktScraper.Core.Entities;
using TransferMarktScraper.WebApi.DTOs;

namespace TransferMarktScraper.WebApi.Services.Interfaces
{
    public interface ITeamServices
    {
        Task<Team> GetTeam(string id);
        Task<IList<Team>> GetTeams();
        Task<Team> AddTeam(Team team);
        Task UpdateTeam(FilterDefinition<Team> filter, UpdateDefinition<Team> update);
        Task DeleteTeams();
        Task<ScrapeResults> ScrapeTeams();
    }
}
