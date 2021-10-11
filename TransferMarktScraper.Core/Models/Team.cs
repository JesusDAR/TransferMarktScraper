using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;


namespace TransferMarktScraper.Core.Models
{
    public class Team
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public double Value { get; set; }
        public TeamTFMData TFMData { get; set; }
        public IEnumerable<ObjectId> Players { get; set; }
    }

    public class TeamTFMData
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
