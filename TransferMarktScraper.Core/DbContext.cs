using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Serilog;
using System;
using TransferMarktScraper.Core.Entities;

namespace TransferMarktScraper.Core
{
    public class DbContext : IDbContext
    {
        private readonly IMongoCollection<Team> _teams;
        private readonly IMongoCollection<Player> _players;
        private readonly IMongoCollection<MarketValue> _marketValues;
        private readonly IMongoCollection<Performance> _performances;

        public DbContext(IOptions<DbConfig> dbConfig, IConfiguration configuration)
        {
            string connectionString = string.Empty;
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environment == "Development")
                connectionString = configuration["ConnectionString"];
            else if (environment == "Production")
                connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            else
                Log.Error("Failed to Load Connection String");
            Log.Information("Connection String - {0}", connectionString);

            MongoClientSettings settings = MongoClientSettings.FromConnectionString(connectionString);
            settings.SslSettings = new SslSettings()
            {
                EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };
            IMongoClient client = new MongoClient(settings);
            IMongoDatabase database = client.GetDatabase(dbConfig.Value.Database_Name);
            Log.Information("Database Name - {0}", dbConfig.Value.Database_Name);

            _teams = database.GetCollection<Team>(dbConfig.Value.Teams_Collection_Name);
            _players = database.GetCollection<Player>(dbConfig.Value.Players_Collection_Name);
            _marketValues = database.GetCollection<MarketValue>(dbConfig.Value.Market_Values_Collection_Name);
            _performances = database.GetCollection<Performance>(dbConfig.Value.Performances_Collection_Name);
            Log.Information("Collections Name - {0} - {1} - {2} - {3}", dbConfig.Value.Teams_Collection_Name, dbConfig.Value.Players_Collection_Name, dbConfig.Value.Market_Values_Collection_Name, dbConfig.Value.Performances_Collection_Name);
        }

        public IMongoCollection<Team> GetTeamsCollection() => _teams;
        public IMongoCollection<Player> GetPlayersCollection() => _players;
        public IMongoCollection<MarketValue> GetMarketValuesCollection() => _marketValues;
        public IMongoCollection<Performance> GetPerformancesCollection() => _performances;
    }
}
