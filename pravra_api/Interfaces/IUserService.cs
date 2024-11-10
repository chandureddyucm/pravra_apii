using System;
using pravra_api.Models;
using System.Threading.Tasks;

namespace pravra_api.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponse<User>> CreateUser(User user);
        Task<ServiceResponse<User>> GetUserByUserId(Guid userId);
        Task<ServiceResponse<User>> GetUser(string email, string password);
        Task<ServiceResponse<IEnumerable<User>>> GetAllUsers();
        Task<ServiceResponse<User>> UpdateUser(User user);
        Task<ServiceResponse<bool>> DeleteUser(Guid userId);
        Task<ServiceResponse<bool>> ToggleUserStatus(Guid userId, bool status);
    }
}