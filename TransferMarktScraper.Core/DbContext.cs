using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TransferMarktScraper.Core.Models;

namespace TransferMarktScraper.Core
{
    public class DbContext : IDbContext
    {
        private readonly IMongoCollection<Team> _teams;
        private readonly IMongoCollection<Player> _players;
        private readonly IMongoCollection<MarketValue> _marketValues;
        private readonly IMongoCollection<Performance> _performances;

        public DbContext(IOptions<DbConfig> dbConfig)
        {
            var client = new MongoClient(dbConfig.Value.Connection_String);
            var database = client.GetDatabase(dbConfig.Value.Database_Name);

            _teams = database.GetCollection<Team>(dbConfig.Value.Teams_Collection_Name);
            _players = database.GetCollection<Player>(dbConfig.Value.Players_Collection_Name);
            _marketValues = database.GetCollection<MarketValue>(dbConfig.Value.Market_Values_Collection_Name);
            _performances = database.GetCollection<Performance>(dbConfig.Value.Performances_Collection_Name);
        }

        public IMongoCollection<Team> GetTeamsCollection() => _teams;
        public IMongoCollection<Player> GetPlayersCollection() => _players;
        public IMongoCollection<MarketValue> GetMarketValuesCollection() => _marketValues;
        public IMongoCollection<Performance> GetPerformancesCollection() => _performances;
    }
}
