﻿using AngleSharp;
using AngleSharp.Dom;
using MongoDB.Driver;
using System;
using System.Collections;
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
    public class TeamServices : ITeamServices
    {
        private readonly IMongoCollection<Team> _teams;

        public TeamServices(IDbContext dbContext)
        {
            _teams = dbContext.GetTeamsCollection();
        }

        public async Task<IList<Team>> GetTeams()
        {
            IList<Team> teams = (await _teams.FindAsync(team => true)).ToList();
            return teams;
        }
        public async Task<Team> GetTeam(string id) => (await _teams.FindAsync(team => team.Id == id)).FirstOrDefault();

        public async Task<Team> AddTeam(Team team)
        {
            await _teams.InsertOneAsync(team);
            return team;
        }

        public async Task UpdateTeam(FilterDefinition<Team> filter, UpdateDefinition<Team> update) => await _teams.UpdateOneAsync(filter, update);

        public async Task DeleteTeams() => await _teams.DeleteManyAsync(team => true);

        public async Task<ScrapeResults> ScrapeTeams()
        {
            ScrapeResults results = new ScrapeResults() { Results = new List<ScrapeResult>() };
            try
            {
                IConfiguration config = Configuration.Default.WithDefaultLoader();
                IBrowsingContext context = BrowsingContext.New(config);

                IDocument doc = await context.OpenAsync(Constants.Transfermarkt + Constants.LaLiga + Constants.SaisonId);

                IHtmlCollection<IElement> rows = doc.QuerySelector("table.items tbody").Children;
                foreach (IElement row in rows)
                {
                    Team team = new Team { TFMData = new TeamTFMData() };
                    ScrapeResult result = new ScrapeResult { };
                    try
                    {
                        team.Name = row.QuerySelector("td:nth-child(2)").TextContent.Trim();

                        string url = row.QuerySelector("td:nth-child(2) a").GetAttribute("href");
                        doc = await context.OpenAsync(Constants.Transfermarkt + url);
                        team.Image = doc.QuerySelector(".dataBild img").GetAttribute("src");

                        string pattern = @"/(.*?)/startseite/verein/(.*?)/";
                        Match match = Regex.Match(url, pattern);
                        team.TFMData.Name = match.Groups[1].Value;
                        team.TFMData.Id = match.Groups[2].Value;

                        string valueString = row.QuerySelector("td:nth-child(8)").TextContent.Split(' ')[0].Trim();
                        if (!double.TryParse(valueString, out double value))
                            value = 0;
                        team.Value = value;
                        await AddTeam(team);

                        result.Message = $"Success obtaining: { team.Name }";
                        result.Code = (int)Constants.Code.Success;
                    }
                    catch (Exception e)
                    {
                        result.Message = $"Error obtaining: { (team.Name != string.Empty ? team.Name : row.Index()) }";
                        result.Code = (int)Constants.Code.Error;
                    }
                    results.Results.Add(result);
                }
            }
            catch (Exception e)
            {
                ScrapeResult result = new ScrapeResult { };
                result.Message = "Error obtaining Teams";
                result.Code = (int)Constants.Code.Error;
                results.Results.Add(result);
            }
            return results;
        }
    }
}