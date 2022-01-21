using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TransferMarktScraper.Core.Entities
{
    public class Player
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string DateOfBirth { get; set; }
        public decimal Height { get; set; }
        public string Foot { get; set; }
        public int Number { get; set; }
        public string Nationality { get; set; }
        public string Position { get; set; }
        public string SigningDate { get; set; }
        public string EndDate { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string MarketValue { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string Performance { get; set; }
        public PlayerTFMData TFMData { get; set; }
    }

    public class PlayerTFMData
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
