using System;
using MongoDB.Driver;
using pravra_api.Interfaces;
using pravra_api.Models;
using Microsoft.Extensions.Options;
using pravra_api.Configurations;

namespace pravra_api.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _users = database.GetCollection<User>("user");
        }

        public async Task CreateUser(User user)
        {
            await _users.InsertOneAsync(user);
        }
    }
}

