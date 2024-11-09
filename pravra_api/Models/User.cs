using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace pravra_api.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid UUID { get; set; }
        
        [BsonElement("FirstName")]
        public required string FirstName { get; set; }
        
        [BsonElement("LastName")]
        public required string LastName { get; set; }
        
        [BsonElement("Gender")]
        public required string Gender { get; set; }
        
        [BsonElement("Mobile")]
        public required string Mobile { get; set; }
        
        [BsonElement("Email")]
        public required string Email { get; set; }
        
        [BsonElement("Password")]
        public required string Password { get; set; }
        
        [BsonElement("IsActive")]
        public bool IsActive { get; set; } = true;
        
        [BsonElement("IsDisabled")]
        public bool IsDisabled { get; set; } = false;
    }
}

