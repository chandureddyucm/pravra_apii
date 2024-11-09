using System;
using pravra_api.Models;
using System.Threading.Tasks;

namespace pravra_api.Interfaces
{
    public interface IUserService
    {
        Task CreateUser(User user);
    }
}

