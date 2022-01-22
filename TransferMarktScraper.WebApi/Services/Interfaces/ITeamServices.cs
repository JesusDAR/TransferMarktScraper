using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransferMarktScraper.Core.Entities;
using TransferMarktScraper.WebApi.DTOs;

namespace TransferMarktScraper.WebApi.Services.Interfaces
{
    public interface ITeamServices
    {
        Task<Team> Get(string id);
        Task<IEnumerable<Team>> GetAll();
        Task<Team> Add(Team team);
        Task Update(FilterDefinition<Team> filter, UpdateDefinition<Team> update);
        Task DeleteAll();
        Task<ScrapeResults> Scrape();
    }
}
