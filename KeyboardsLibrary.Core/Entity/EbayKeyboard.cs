using MongoDB.Bson.Serialization.Attributes;

namespace KeyboardsLibrary.Core.Entity
{
    public class EbayKeyboard
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string EbayId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Link { get; set; }
        public string Standing { get; set; }
        public string Image { get; set; }
    }
}