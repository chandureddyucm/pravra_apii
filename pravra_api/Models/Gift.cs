using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace pravra_api.Models
{
    public class Gift
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("GiftId")]
        [BsonRepresentation(BsonType.String)]
        public Guid GiftId { get; set; }

        [BsonElement("Name")]
        public required string Name { get; set; }

        [BsonElement("Description")]
        public required string Description { get; set; }

        [BsonElement("Category")]
        public required string Category { get; set; }

        [BsonElement("Subcategory")]
        public required string Subcategory { get; set; }

        [BsonElement("ImageSrc")]
        public required string ImageSrc { get; set; }//ToDo

        [BsonElement("Price")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }

        [BsonElement("Availability")]
        public bool Availability { get; set; } = true;

        // [BsonElement("Filters")]
        // public required Dictionary<string, List<string>> Filters { get; set; }

        [BsonElement("IsDeleted")]
        public bool IsDeleted { get; set; } = false;

    }
}
