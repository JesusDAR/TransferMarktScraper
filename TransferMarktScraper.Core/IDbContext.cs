using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferMarktScraper.Core.Models;

namespace TransferMarktScraper.Core
{
    public interface IDbContext
    {
        IMongoCollection<Team> GetTeamsCollection();
        IMongoCollection<Player> GetPlayersCollection();
        IMongoCollection<MarketValue> GetMarketValuesCollection();
        IMongoCollection<Performance> GetPerformancesCollection();
    }
}
