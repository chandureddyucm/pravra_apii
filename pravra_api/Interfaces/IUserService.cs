using System;
using pravra_api.Models;
using System.Threading.Tasks;

namespace pravra_api.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponse<User>> CreateUser(User user);
        Task<ServiceResponse<User>> GetUserByUserId(string userId);
        Task<ServiceResponseLogin<User>> Login(string email, string password);
        Task<ServiceResponse<IEnumerable<User>>> GetAllUsers();
        Task<ServiceResponse<User>> UpdateUser(string userId, string firstName, string lastName, string mobile);
        Task<ServiceResponse<bool>> DeleteUser(string userId);
        Task<ServiceResponse<bool>> ToggleUserStatus(string userId, bool status);
        Task<ServiceResponse<bool>> AddSubscription(Subscription subscriber);
        Task<ServiceResponse<IEnumerable<Subscription>>> GetAllSubscribers();
    }
}