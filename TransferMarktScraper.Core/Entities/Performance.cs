using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferMarktScraper.Core.Entities
{
    public class Performance
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int MatchesCalled { get; set; }
        public int MatchesPlayed { get; set; }
        public decimal PointsPerMatch { get; set; }
        public int Goals { get; set; }
        public int GoalAssists { get; set; }
        public int OwnGoals { get; set; }
        public int SubtitutionsIn { get; set; }
        public int SubtitutionsOut { get; set; }
        public int YellowCards { get; set; }
        public int SecondYellowCards { get; set; }
        public int RedCards { get; set; }
        public int PenaltiesScored { get; set; }
        public int MinsPerGoal { get; set; }
        public int MinsPlayed { get; set; }

        public int GoalsConceded { get; set; }
        public int MatchesUnbeaten { get; set; }
        public IList<PerformanceItem> PerformanceItems { get; set; }
    }

    public class PerformanceItem
    {
        public string Season { get; set; }
        public string Competition { get; set; }
        public string Team { get; set; }
        public int MatchesCalled { get; set; }
        public int MatchesPlayed { get; set; }
        public decimal PointsPerMatch { get; set; }
        public int Goals { get; set; }
        public int GoalAssists { get; set; }
        public int OwnGoals { get; set; }
        public int SubtitutionsIn { get; set; }
        public int SubtitutionsOut { get; set; }
        public int YellowCards { get; set; }
        public int SecondYellowCards { get; set; }
        public int RedCards { get; set; }
        public int PenaltiesScored { get; set; }
        public int MinsPerGoal { get; set; }
        public int MinsPlayed { get; set; }

        public int GoalsConceded { get; set; }
        public int MatchesUnbeaten { get; set; }

    }

}
