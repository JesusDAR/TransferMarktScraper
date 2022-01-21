using AngleSharp;
using AngleSharp.Dom;
using System.Threading.Tasks;
using System.Linq;
using TransferMarktScraper.Core;
using TransferMarktScraper.Core.Entities;
using TransferMarktScraper.WebApi.Services.Interfaces;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using MongoDB.Driver;
using TransferMarktScraper.WebApi.DTOs;
using System;

namespace TransferMarktScraper.WebApi.Services
{
    public class MarketValueServices : IMarketValueServices
    {
        private readonly IMongoCollection<MarketValue> _marketValues;
        private readonly IPlayerServices _playerServices;

        public MarketValueServices(IDbContext dbContext, IPlayerServices playerServices)
        {
            _marketValues = dbContext.GetMarketValuesCollection();
            _playerServices = playerServices;
        }
        public async Task<MarketValue> GetByPlayerId(string id)
        {
            Player player = await _playerServices.Get(id);
            FilterDefinition<MarketValue> filter = Builders<MarketValue>.Filter.Eq(mv => mv.Id, player.MarketValue);
            MarketValue marketValue = (await _marketValues.FindAsync(filter)).FirstOrDefault();
            return marketValue;
        }
        public async Task<MarketValue> Add(MarketValue marketValue)
        {
            await _marketValues.InsertOneAsync(marketValue);
            return marketValue;
        }

        public async Task<Player> AddToPlayer(Player player, MarketValue marketValue)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, player.Id);
            UpdateDefinition<Player> update = Builders<Player>.Update.Set(p => p.MarketValue, marketValue.Id);
            await _playerServices.Update(filter, update);
            return player;
        }

        public async Task DeleteByPlayerId(string id)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAllByPlayerIds(IEnumerable<string> ids)
        {
            throw new NotImplementedException();
        }

        public async Task<ScrapeResults> ScrapeByPlayerId(string id)
        {
            ScrapeResults results = new ScrapeResults() { Results = new List<ScrapeResult>() };
            ScrapeResult result = new ScrapeResult {};
            Player player = new Player();
            MarketValue marketValue = new MarketValue() { Data = new List<MarketValueData>()};
            try
            {
                IConfiguration config = Configuration.Default.WithDefaultLoader();
                IBrowsingContext context = BrowsingContext.New(config);

                player = await _playerServices.Get(id);

                IDocument doc = await context.OpenAsync(Constants.Transfermarkt + "/" + player.TFMData.Name + "/marktwertverlauf/spieler/" + player.TFMData.Id);

                string script = doc.Scripts.Last().InnerHtml;
                string valuesStringUnescaped = Regex.Match(script, @"\'series\':\[(.*?)\],", RegexOptions.Singleline).Groups[1].Value;
                string valuesString = Regex.Unescape(valuesStringUnescaped);

                JObject json = JObject.Parse(valuesString);
                marketValue.Data.AddRange(json["data"].Select(item => new MarketValueData
                {
                    Value = item["mw"].ToString(),
                    Date = item["datum_mw"].ToString(),
                    Team = item["verein"].ToString()
                }));

                await Add(marketValue);
                await AddToPlayer(player, marketValue);

                result.Message = $"Success fetching: { player.Name } market value";
                result.Code = (int)Constants.Code.Success;
                results.Results.Add(result);
            }
            catch (Exception e)
            {
                result.Message = $"Error fetching: { (player != null ? player.Name : id)} market value";
                result.Code = (int)Constants.Code.Error;
                results.Results.Add(result);
            }
            return results;
        }
    }
}
