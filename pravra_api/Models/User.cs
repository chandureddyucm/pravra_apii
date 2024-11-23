using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace pravra_api.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("UserId")]
        [BsonRepresentation(BsonType.String)]
        public Guid UserId { get; set; }

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
        
        [BsonElement("IsAdmin")]
        public bool IsAdmin { get; set; } = false;

        [BsonElement("IsActive")]
        public bool IsActive { get; set; } = true;

        [BsonElement("IsVerified")]
        public bool IsVerified { get; set; } = true;
    }
}