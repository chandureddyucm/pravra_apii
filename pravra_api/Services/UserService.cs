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
                {
                    response.Message = "Mobile or Email is already in use.";
                    response.Success = false;
                    return response;
                }

                // If not, insert the new user into the database
                await _users.InsertOneAsync(user);

                response.Data = user;
                response.Success = true;
                response.Message = "User created successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }

        public async Task<ServiceResponse<User>> GetUserByUserId(string userId)
        {
            var response = new ServiceResponse<User>();
            try
            {
                response.Data = await _users.Find(u => u.UserId.ToString() == userId).FirstOrDefaultAsync();
                if (response.Data == null)
                {
                    response.Message = $"User not found with userId:{userId}";
                    response.Success = false;
                }
                else
                    response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }

        public async Task<ServiceResponse<User>> Login(string email, string password)
        {
            var response = new ServiceResponse<User>();
            try
            {
                response.Data = await _users.Find(u => u.Email == email && u.Password == password).FirstOrDefaultAsync();
                if (response.Data == null)
                {
                    response.Message = $"User not found or wrong credentials";
                    response.Success = false;
                }
                else
                {
                    var token = _jwtHelper.GenerateToken(response.Data);
                    response.BearerToken = token;
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }

        public async Task<ServiceResponse<IEnumerable<User>>> GetAllUsers()
        {
            var response = new ServiceResponse<IEnumerable<User>>();
            try
            {
                response.Data = await _users.Find(_ => true).ToListAsync();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
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
                {
                    response.Message = "Updated user details successfully";
                    response.Success = true;
                }
                else
                {
                    response.Message = "User not found or no changes made.";
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteUser(string userId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var delete = Builders<User>.Update.Set(u => u.IsActive, false);
                var deleteResult = await _users.UpdateOneAsync(u => u.UserId.ToString() == userId, delete);
                if (deleteResult.ModifiedCount > 0)
                {
                    response.Message = "User deleted successfully";
                    response.Success = true;
                }
                else
                {
                    response.Message = "User not found";
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
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
                {
                    response.Message = "User status toggled successfully";
                    response.Success = true;
                }
                else
                {
                    response.Message = "User not found";
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }
    }
}