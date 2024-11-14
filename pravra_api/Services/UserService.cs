using System;
using MongoDB.Driver;
using pravra_api.Interfaces;
using pravra_api.Models;
using Microsoft.Extensions.Options;
using pravra_api.Configurations;
using Microsoft.AspNetCore.Http.HttpResults;
using pravra_api.Extensions;

namespace pravra_api.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _users;
        private readonly JwtHelper _jwtHelper;
        private readonly IConfiguration _configuration;

        public UserService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings, IConfiguration configuration)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _users = database.GetCollection<User>("user");

            _configuration = configuration;
            _jwtHelper = new JwtHelper(_configuration);
        }

        public async Task<ServiceResponse<User>> CreateUser(User user)
        {
            var response = new ServiceResponse<User>();
            try
            {
                // Check if the Mobile or Email already exists in the database
                var existingUser = await _users.Find(u => u.Mobile == user.Mobile || u.Email == user.Email).FirstOrDefaultAsync();

                if (existingUser != null)
                    return response.SetResponse(false, "Mobile or Email is already in use.");

                // If not, insert the new user into the database
                await _users.InsertOneAsync(user);
                return response.SetResponse(true, "User created successfully", user);
            }
            catch (Exception ex)
            {
                return response.SetResponseWithEx(ex.Message);
            }
        }

        public async Task<ServiceResponse<User>> GetUserByUserId(string userId)
        {
            var response = new ServiceResponse<User>();
            try
            {
                User user = await _users.Find(u => u.UserId.ToString() == userId).FirstOrDefaultAsync();
                if (user == null)
                    return response.SetResponse(false, $"User not found with userId:{userId}");
                else
                    return response.SetResponse(true, user);
            }
            catch (Exception ex)
            {
                return response.SetResponseWithEx(ex.Message);
            }
        }

        public async Task<ServiceResponseLogin<User>> Login(string email, string password)
        {
            var response = new ServiceResponseLogin<User>();
            try
            {
                response.Data = await _users.Find(u => u.Email == email && u.Password == password).FirstOrDefaultAsync();
                if (response.Data == null)
                    return response.SetResponse(false, $"User not found or wrong credentials");
                else
                {
                    var token = _jwtHelper.GenerateToken(response.Data);
                    return response.SetResponse(true, "User Logged In Successfully", token);
                }
            }
            catch (Exception ex)
            {
                return response.SetResponseWithEx(ex.Message);
            }
        }

        public async Task<ServiceResponse<IEnumerable<User>>> GetAllUsers()
        {
            var response = new ServiceResponse<IEnumerable<User>>();
            try
            {
                List<User> users = await _users.Find(_ => true).ToListAsync();
                return response.SetResponse(true, users);
            }
            catch (Exception ex)
            {
                return response.SetResponseWithEx(ex.Message);
            }
        }

        public async Task<ServiceResponse<User>> UpdateUser(string userId, string firstName, string lastName, string mobile)
        {
            var response = new ServiceResponse<User>();
            try
            {
                var update = Builders<User>.Update.Set(u => u.FirstName, firstName).Set(u => u.LastName, lastName).Set(u => u.Mobile, mobile);
                var updateResult = await _users.UpdateOneAsync(u => u.UserId.ToString() == userId, update);
                //var updateResult = await _users.ReplaceOneAsync(u => u.UserId == user.UserId, user);
                if (updateResult.ModifiedCount > 0)
                    return response.SetResponse(true, "Updated user details successfully");
                else
                    return response.SetResponse(false, "User not found or no changes made.");
            }
            catch (Exception ex)
            {
                return response.SetResponseWithEx(ex.Message);
            }
        }

        public async Task<ServiceResponse<bool>> DeleteUser(string userId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var delete = Builders<User>.Update.Set(u => u.IsActive, false);
                var deleteResult = await _users.UpdateOneAsync(u => u.UserId.ToString() == userId, delete);
                if (deleteResult.ModifiedCount > 0)
                    return response.SetResponse(true, "User deleted successfully");
                else
                    return response.SetResponse(false, "User not found");
            }
            catch (Exception ex)
            {
                return response.SetResponseWithEx(ex.Message);
            }
        }

        public async Task<ServiceResponse<bool>> ToggleUserStatus(string userId, bool status)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                // Update definition to set IsActive to false
                var update = Builders<User>.Update.Set(u => u.IsActive, status);

                // Update only the IsActive field for the user with the UserId
                var updateResult = await _users.UpdateOneAsync(u => u.UserId.ToString() == userId, update);

                if (updateResult.ModifiedCount > 0)
                    return response.SetResponse(true, "User status toggled successfully");
                else
                    return response.SetResponse(false, "User not found");
            }
            catch (Exception ex)
            {
                return response.SetResponseWithEx(ex.Message);
            }
        }
    }
}