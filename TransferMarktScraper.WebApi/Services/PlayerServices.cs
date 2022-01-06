using AngleSharp;
using AngleSharp.Dom;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TransferMarktScraper.Core;
using TransferMarktScraper.Core.Models;
using TransferMarktScraper.WebApi.DTOs;
using TransferMarktScraper.WebApi.Services.Interfaces;

namespace TransferMarktScraper.WebApi.Services
{
    public class PlayerServices : IPlayerServices
    {
        private readonly IMongoCollection<Player> _players;
        private readonly ITeamServices _teamServices;
        
        public PlayerServices(IDbContext dbContext, ITeamServices teamServices)
        {
            _players = dbContext.GetPlayersCollection();
            _teamServices = teamServices;
        }
        public async Task<IEnumerable<Player>> GetPlayers() => (await _players.FindAsync(p => true)).ToEnumerable();
        public async Task<Player> GetPlayer(string id) => (await _players.FindAsync(p => p.Id == id)).FirstOrDefault();
        public async Task<IEnumerable<Player>> GetPlayersByTeamId(string id)
        {
            Team team = await _teamServices.GetTeam(id);
            FilterDefinition<Player> filter = Builders<Player>.Filter.In(p => p.Id, team.Players.Select(s => s.ToString()));
            IEnumerable<Player> players = (await _players.FindAsync(filter)).ToEnumerable();
            return players;
        }

        public async Task<Player> AddPlayer(Player player)
        {
            await _players.InsertOneAsync(player);
            return player;
        }

        public async Task<Team> AddPlayersToTeam(Team team, IEnumerable<Player> players)
        {
            FilterDefinition<Team> filter = Builders<Team>.Filter.Eq(t => t.Id, team.Id);
            UpdateDefinition<Team> update = Builders<Team>.Update.Set(t => t.Players, players.Select(p => p.Id));
            await _teamServices.UpdateTeam(filter, update);
            return team;
        }

        public async Task UpdatePlayer(FilterDefinition<Player> filter, UpdateDefinition<Player> update) => await _players.UpdateOneAsync(filter, update);

        public async Task DeletePlayers() => await _players.DeleteManyAsync(p => true);
        public async Task DeletePlayersByTeamId(string id)
        {
            Team team = await _teamServices.GetTeam(id);
            FilterDefinition<Player> filter = Builders<Player>.Filter.In(p => p.Id, team.Players);
            await _players.DeleteManyAsync(filter);
        }

        public async Task<ScrapeResults> ScrapePlayersByTeamId(string id)
        {
            ScrapeResults results = new ScrapeResults() { Results = new List<ScrapeResult>() };
            Team team = new Team();
            try
            {
                team = await _teamServices.GetTeam(id);
                IConfiguration config = Configuration.Default.WithDefaultLoader();
                IBrowsingContext context = BrowsingContext.New(config);

                IDocument doc = await context.OpenAsync(Constants.Transfermarkt + "/" + team.TFMData.Name + "/kader/verein/" + team.TFMData.Id + "/saison_id" + Constants.SaisonId + Constants.Ampliado);

                string url = doc.QuerySelector(".tm-tabs > a:first-of-type").GetAttribute("href");
                string pattern = @"/(.*?)/";
                Match match = Regex.Match(url, pattern);
                string nameTFM = match.Groups[1].Value;

                if (!nameTFM.Equals(team.TFMData.Name))
                {
                    FilterDefinition<Team> filter = Builders<Team>.Filter.Eq(t => t.Id, team.Id);
                    UpdateDefinition<Team> update = Builders<Team>.Update.Set(team => team.TFMData.Name, nameTFM);
                    await _teamServices.UpdateTeam(filter, update);
                }

                doc = await context.OpenAsync(Constants.Transfermarkt + "/" + team.TFMData.Name + "/kader/verein/" + team.TFMData.Id + "/saison_id" + Constants.SaisonId + Constants.Ampliado);

                IHtmlCollection<IElement> rows = doc.QuerySelector("table.items tbody").Children;
                List<Player> players = new List<Player>();
                foreach (IElement row in rows)
                {
                    Player player = new Player() { TFMData = new PlayerTFMData()};
                    ScrapeResult result = new ScrapeResult { };
                    try
                    {
                        player.Name = row.QuerySelector("td:nth-child(2) table td:nth-child(2) a").TextContent;
                        string numberString = row.QuerySelector("td:first-child div").TextContent;
                        if (!int.TryParse(numberString, out int number))
                            number = 0;
                        player.Number = number;

                        url = row.QuerySelector("td:nth-child(2) table td:nth-child(2) a").GetAttribute("href");
                        pattern = @"/(.*?)/profil/spieler/(.*)";
                        match = Regex.Match(url, pattern);
                        player.TFMData.Name = match.Groups[1].Value;
                        player.TFMData.Id = match.Groups[2].Value;

                        player.Position = row.QuerySelector("td:nth-child(2) table tr:nth-child(2) td").TextContent;

                        string date = row.QuerySelector("td:nth-child(3)").TextContent;
                        player.DateOfBirth = date.Split()[0];
                        string ageString = date.Split()[1].Replace("(", string.Empty).Replace(")", string.Empty);
                        if (!int.TryParse(ageString, out int age))
                            age = 0;
                        player.Age = age;

                        player.Nationality = string.Join(", ", row.QuerySelectorAll("td:nth-child(4) img").Select(s => s.GetAttribute("title")).ToArray());

                        string heightString = row.QuerySelector("td:nth-child(5)").TextContent.Split()[0].Replace(',', '.');
                        if (!decimal.TryParse(heightString, out decimal height))
                            height = 0;
                        player.Height = height;

                        player.Foot = row.QuerySelector("td:nth-child(6)").TextContent;
                        player.SigningDate = row.QuerySelector("td:nth-child(7)").TextContent;
                        player.EndDate = row.QuerySelector("td:nth-child(9)").TextContent;

                        await AddPlayer(player);
                        result.Message = $" { team.Name } - Success fetching: { player.Name }";
                        result.Code = (int)Constants.Code.Error;
                        players.Add(player);
                    }
                    catch (Exception e)
                    {
                        result.Message = $" { team.Name } - Error fetching: { (player.Name != string.Empty ? player.Name : row.Index()) }";
                        result.Code = (int)Constants.Code.Error;
                    }
                    results.Results.Add(result);
                }
                await AddPlayersToTeam(team, players);
            }
            catch (Exception e)
            {
                ScrapeResult result = new ScrapeResult { };
                result.Message = $"{ (team.Name != null ? team.Name : id) } - Error fetching Players";
                result.Code = (int)Constants.Code.Error;
                results.Results.Add(result);
            }
            return results;
        }
    }
}
