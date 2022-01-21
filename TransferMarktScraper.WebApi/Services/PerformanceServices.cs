using AngleSharp;
using AngleSharp.Dom;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransferMarktScraper.Core;
using TransferMarktScraper.Core.Entities;
using TransferMarktScraper.WebApi.DTOs;
using TransferMarktScraper.WebApi.Services.Interfaces;

namespace TransferMarktScraper.WebApi.Services
{
    public class PerformanceServices : IPerformanceServices
    {
        private readonly IMongoCollection<Performance> _performances;
        private readonly IPlayerServices _playerServices;

        public PerformanceServices(IDbContext dbContext, IPlayerServices playerServices)
        {
            _performances = dbContext.GetPerformancesCollection();
            _playerServices = playerServices;
        }
        public async Task<Performance> GetByPlayerId(string id)
        {
            Player player = await _playerServices.Get(id);
            FilterDefinition<Performance> filter = Builders<Performance>.Filter.Eq(per => per.Id, player.Performance);
            Performance performance = (await _performances.FindAsync(filter)).FirstOrDefault();
            return performance;
        }
        public async Task<Performance> Add(Performance performance)
        {
            await _performances.InsertOneAsync(performance);
            return performance;
        }
        public async Task<Player> AddToPlayer(Player player, Performance performance)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, player.Id);
            UpdateDefinition<Player> update = Builders<Player>.Update.Set(p => p.Performance, performance.Id);
            await _playerServices.Update(filter, update);
            return player;
        }

        public async Task DeleteAll() => await _performances.DeleteManyAsync(p => true);
        public async Task<ScrapeResults> ScrapePlayerId(string id)
        {
            ScrapeResults results = new ScrapeResults() { Results = new List<ScrapeResult>() };
            ScrapeResult result = new ScrapeResult { };
            Player player = new Player();
            Performance performance = new Performance() { PerformanceItems = new List<PerformanceItem>()};
            try
            {
                IConfiguration config = Configuration.Default.WithDefaultLoader();
                IBrowsingContext context = BrowsingContext.New(config);

                player = await _playerServices.Get(id);

                IDocument doc = await context.OpenAsync(Constants.Transfermarkt + "/" + player.TFMData.Name + "/leistungsdatendetails/spieler/" + player.TFMData.Id + Constants.Ampliado);

                if (player.Position.ToLower().Equals("portero"))
                    ScrapePerformanceGoalkeeper(doc, performance);
                else
                    ScrapePerformanceFieldPlayer(doc, performance);

                await Add(performance);
                await AddToPlayer(player, performance);
                result.Message = $"Success fetching: { player.Name } performance";
                result.Code = (int)Constants.Code.Success;
                results.Results.Add(result);
            }
            catch (Exception e)
            {
                result.Message = $"Error fetching: { (player != null ? player.Name : id)} performance";
                result.Code = (int)Constants.Code.Error;
                results.Results.Add(result);
            }
            return results;
        }

        private void ScrapePerformanceFieldPlayer(IDocument doc, Performance performance)
        {
            IHtmlCollection<IElement> total = doc.QuerySelector("table.items > tfoot > tr").Children;

            int matchesCalled;
            int.TryParse(total[4].TextContent, out matchesCalled);
            performance.MatchesCalled = matchesCalled;

            int matchesPlayed;
            int.TryParse(total[5].TextContent, out matchesPlayed);
            performance.MatchesPlayed = matchesPlayed;

            decimal pointsPerMatch;
            decimal.TryParse(total[6].TextContent, out pointsPerMatch);
            performance.PointsPerMatch = pointsPerMatch;

            int goals;
            int.TryParse(total[7].TextContent, out goals);
            performance.Goals = goals;

            int goalAssists;
            int.TryParse(total[8].TextContent, out goalAssists);
            performance.GoalAssists = goalAssists;

            int ownGoals;
            int.TryParse(total[9].TextContent, out ownGoals);
            performance.OwnGoals = ownGoals;

            int subtitutionsIn;
            int.TryParse(total[10].TextContent, out subtitutionsIn);
            performance.SubtitutionsIn = subtitutionsIn;

            int subtitutionsOut;
            int.TryParse(total[11].TextContent, out subtitutionsOut);
            performance.SubtitutionsOut = subtitutionsOut;

            int yellowCards;
            int.TryParse(total[12].TextContent, out yellowCards);
            performance.YellowCards = yellowCards;

            int secondYellowCards;
            int.TryParse(total[13].TextContent, out secondYellowCards);
            performance.SecondYellowCards = secondYellowCards;

            int redCards;
            int.TryParse(total[14].TextContent, out redCards);
            performance.RedCards = redCards;

            int penaltiesScored;
            int.TryParse(total[15].TextContent, out penaltiesScored);
            performance.PenaltiesScored = penaltiesScored;

            int minsPlayed;
            string content = total[16].TextContent;
            if (content.Contains("'"))
                content = content.Replace("'", string.Empty);
            if (content.Contains("."))
                content = content.Replace(".", string.Empty);
            int.TryParse(content, out minsPlayed);
            performance.MinsPlayed = minsPlayed;

            IHtmlCollection<IElement> rows = doc.QuerySelector("table.items > tbody").Children;
            foreach (IElement row in rows)
            {
                PerformanceItem performanceItem = new PerformanceItem();
                performanceItem.Season = row.QuerySelector("td:first-of-type").TextContent;
                performanceItem.Competition = row.QuerySelector("td:nth-child(3)").TextContent;
                performanceItem.Team = row.QuerySelector("td:nth-child(4) > a > img").GetAttribute("alt");

                matchesCalled = 0;
                int.TryParse(row.QuerySelector("td:nth-child(5)").TextContent, out matchesCalled);
                performanceItem.MatchesCalled = matchesCalled;

                matchesPlayed = 0;
                int.TryParse(row.QuerySelector("td:nth-child(6)").TextContent, out matchesPlayed);
                performanceItem.MatchesPlayed = matchesPlayed;

                pointsPerMatch = 0;
                decimal.TryParse(row.QuerySelector("td:nth-child(7)").TextContent, out pointsPerMatch);
                performanceItem.PointsPerMatch = pointsPerMatch;

                goals = 0;
                int.TryParse(row.QuerySelector("td:nth-child(8)").TextContent, out goals);
                performanceItem.Goals = goals;

                goalAssists = 0;
                int.TryParse(row.QuerySelector("td:nth-child(9)").TextContent, out goalAssists);
                performanceItem.GoalAssists = goalAssists;

                ownGoals = 0;
                int.TryParse(row.QuerySelector("td:nth-child(10)").TextContent, out ownGoals);
                performanceItem.OwnGoals = ownGoals;

                subtitutionsIn = 0;
                int.TryParse(row.QuerySelector("td:nth-child(11)").TextContent, out subtitutionsIn);
                performanceItem.SubtitutionsIn = subtitutionsIn;

                subtitutionsOut = 0;
                int.TryParse(row.QuerySelector("td:nth-child(12)").TextContent, out subtitutionsOut);
                performanceItem.SubtitutionsOut = subtitutionsOut;

                yellowCards = 0;
                int.TryParse(row.QuerySelector("td:nth-child(13)").TextContent, out yellowCards);
                performanceItem.YellowCards = yellowCards;

                secondYellowCards = 0;
                int.TryParse(row.QuerySelector("td:nth-child(14)").TextContent, out secondYellowCards);
                performanceItem.SecondYellowCards = secondYellowCards;

                redCards = 0;
                int.TryParse(row.QuerySelector("td:nth-child(15)").TextContent, out redCards);
                performanceItem.RedCards = redCards;

                penaltiesScored = 0;
                int.TryParse(row.QuerySelector("td:nth-child(16)").TextContent, out penaltiesScored);
                performanceItem.PenaltiesScored = penaltiesScored;


                minsPlayed = 0;
                content = row.QuerySelector("td:nth-child(17)").TextContent;
                if (content.Contains("'"))
                    content = content.Replace("'", string.Empty);
                if (content.Contains("."))
                    content = content.Replace(".", string.Empty);
                int.TryParse(content, out minsPlayed);
                performanceItem.MinsPlayed = minsPlayed;

                performance.PerformanceItems.Add(performanceItem);
            }
        }

        private void ScrapePerformanceGoalkeeper(IDocument doc, Performance performance)
        {
            IHtmlCollection<IElement> total = doc.QuerySelector("table.items > tfoot > tr").Children;

            int matchesCalled;
            int.TryParse(total[4].TextContent, out matchesCalled);
            performance.MatchesCalled = matchesCalled;

            int matchesPlayed;
            int.TryParse(total[5].TextContent, out matchesPlayed);
            performance.MatchesPlayed = matchesPlayed;

            decimal pointsPerMatch;
            decimal.TryParse(total[6].TextContent, out pointsPerMatch);
            performance.PointsPerMatch = pointsPerMatch;

            int goals;
            int.TryParse(total[7].TextContent, out goals);
            performance.Goals = goals;

            int ownGoals;
            int.TryParse(total[8].TextContent, out ownGoals);
            performance.OwnGoals = ownGoals;

            int subtitutionsIn;
            int.TryParse(total[9].TextContent, out subtitutionsIn);
            performance.SubtitutionsIn = subtitutionsIn;

            int subtitutionsOut;
            int.TryParse(total[10].TextContent, out subtitutionsOut);
            performance.SubtitutionsOut = subtitutionsOut;

            int yellowCards;
            int.TryParse(total[11].TextContent, out yellowCards);
            performance.YellowCards = yellowCards;

            int secondYellowCards;
            int.TryParse(total[12].TextContent, out secondYellowCards);
            performance.SecondYellowCards = secondYellowCards;

            int redCards;
            int.TryParse(total[13].TextContent, out redCards);
            performance.RedCards = redCards;

            int goalsConceded;
            int.TryParse(total[14].TextContent, out goalsConceded);
            performance.GoalsConceded = goalsConceded;

            int matchesUnbeaten;
            int.TryParse(total[15].TextContent, out matchesUnbeaten);
            performance.MatchesUnbeaten = matchesUnbeaten;

            int minsPlayed;
            string content = total[16].TextContent;
            if (content.Contains("'"))
                content = content.Replace("'", string.Empty);
            if (content.Contains("."))
                content = content.Replace(".", string.Empty);
            int.TryParse(content, out minsPlayed);
            performance.MinsPlayed = minsPlayed;

            IHtmlCollection<IElement> rows = doc.QuerySelector("table.items > tbody").Children;
            foreach (IElement row in rows)
            {
                PerformanceItem performanceItem = new PerformanceItem();
                performanceItem.Season = row.QuerySelector("td:first-of-type").TextContent;
                performanceItem.Competition = row.QuerySelector("td:nth-child(3)").TextContent;
                performanceItem.Team = row.QuerySelector("td:nth-child(4) > a > img").GetAttribute("alt");

                matchesCalled = 0;
                int.TryParse(row.QuerySelector("td:nth-child(5)").TextContent, out matchesCalled);
                performanceItem.MatchesCalled = matchesCalled;

                matchesPlayed = 0;
                int.TryParse(row.QuerySelector("td:nth-child(6)").TextContent, out matchesPlayed);
                performanceItem.MatchesPlayed = matchesPlayed;

                pointsPerMatch = 0;
                decimal.TryParse(row.QuerySelector("td:nth-child(7)").TextContent, out pointsPerMatch);
                performanceItem.PointsPerMatch = pointsPerMatch;

                goals = 0;
                int.TryParse(row.QuerySelector("td:nth-child(8)").TextContent, out goals);
                performanceItem.Goals = goals;

                ownGoals = 0;
                int.TryParse(row.QuerySelector("td:nth-child(9)").TextContent, out ownGoals);
                performanceItem.OwnGoals = ownGoals;

                subtitutionsIn = 0;
                int.TryParse(row.QuerySelector("td:nth-child(10)").TextContent, out subtitutionsIn);
                performanceItem.SubtitutionsIn = subtitutionsIn;

                subtitutionsOut = 0;
                int.TryParse(row.QuerySelector("td:nth-child(11)").TextContent, out subtitutionsOut);
                performanceItem.SubtitutionsOut = subtitutionsOut;

                yellowCards = 0;
                int.TryParse(row.QuerySelector("td:nth-child(12)").TextContent, out yellowCards);
                performanceItem.YellowCards = yellowCards;

                secondYellowCards = 0;
                int.TryParse(row.QuerySelector("td:nth-child(13)").TextContent, out secondYellowCards);
                performanceItem.SecondYellowCards = secondYellowCards;

                redCards = 0;
                int.TryParse(row.QuerySelector("td:nth-child(14)").TextContent, out redCards);
                performanceItem.RedCards = redCards;

                goalsConceded = 0;
                int.TryParse(row.QuerySelector("td:nth-child(15)").TextContent, out goalsConceded);
                performanceItem.GoalsConceded = goalsConceded;

                matchesUnbeaten = 0;
                int.TryParse(row.QuerySelector("td:nth-child(16)").TextContent, out matchesUnbeaten);
                performanceItem.MatchesUnbeaten = matchesUnbeaten;

                minsPlayed = 0;
                content = row.QuerySelector("td:nth-child(17)").TextContent;
                if (content.Contains("'"))
                    content = content.Replace("'", string.Empty);
                if (content.Contains("."))
                    content = content.Replace(".", string.Empty);
                int.TryParse(content, out minsPlayed);
                performanceItem.MinsPlayed = minsPlayed;

                performance.PerformanceItems.Add(performanceItem);
            }
        }
    }
}
