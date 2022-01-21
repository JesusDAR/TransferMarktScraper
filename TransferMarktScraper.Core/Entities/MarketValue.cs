using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using MongoDB.Bson;

namespace TransferMarktScraper.Core.Entities
{ 
    public class MarketValue
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public List<MarketValueData> Data { get; set; }

    }
    public class MarketValueData
    {
        public string Date { get; set; }
        public string Value { get; set; }
        public string Team { get; set; }
    }
}
