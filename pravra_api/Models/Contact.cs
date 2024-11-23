using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace pravra_api.Models
{
    public class Contact
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("ContactId")]
        [BsonRepresentation(BsonType.String)]
        public Guid ContactId { get; set; }

        [BsonElement("Tag")]
        public required string Tag { get; set; }

        [BsonElement("Contacts")]
        public required Contacts Contacts { get; set; }
    }

    public class Contacts
    {
        [BsonElement("Type")]
        public required string Type { get; set; }

        [BsonElement("Value")]
        public required string Value { get; set; }
    }
}
