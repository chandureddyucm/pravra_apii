using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace pravra_api.Models
{
    public class Subscription
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("SubscriptionId")]
        [BsonRepresentation(BsonType.String)]
        public Guid SubscriptionId { get; set; }

        [BsonElement("Email")]
        public required string Email { get; set; }

        [BsonElement("IsActive")]
        public bool IsActive { get; set; } = true;
    }
}
