using System;
using pravra_api.Models;

namespace pravra_api.Services
{
    public interface IContactService
    {
        Task<ServiceResponse<IEnumerable<Contact>>> GetAllContacts();
        Task<ServiceResponse<Contact>> GetContactById(string contactId);
        Task<ServiceResponse<Contact>> AddContact(Contact contact);
        Task<ServiceResponse<Contact>> UpdateContact(string contactId, Contact contact);
        Task<ServiceResponse<bool>> DeleteContact(string contactId);
    }
}