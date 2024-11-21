using System;
using MongoDB.Driver;
using pravra_api.Models;
using Microsoft.Extensions.Options;
using pravra_api.Configurations;
using pravra_api.Extensions;

namespace pravra_api.Services
{
    public class ContactService : IContactService
    {
        private readonly IMongoCollection<Contact> _contacts;
        private readonly JwtHelper _jwtHelper;
        private readonly IConfiguration _configuration;

        public ContactService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings, IConfiguration configuration)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _contacts = database.GetCollection<Contact>("Contacts");

            _configuration = configuration;
            _jwtHelper = new JwtHelper(_configuration);
        }

        public async Task<ServiceResponse<IEnumerable<Contact>>> GetAllContacts()
        {
            var response = new ServiceResponse<IEnumerable<Contact>>();
            try
            {
                List<Contact> contacts = await _contacts.Find(_ => true).ToListAsync();
                return response.SetResponse(true, contacts);
            }
            catch (Exception ex)
            {
                return response.SetResponseWithEx(ex.Message);
            }
        }

        public async Task<ServiceResponse<Contact>> GetContactById(string contactId)
        {
            var response = new ServiceResponse<Contact>();
            try
            {
                Contact contact = await _contacts.Find(u => u.ContactId.ToString() == contactId).FirstOrDefaultAsync();
                if (contact == null)
                    return response.SetResponse(false, $"Gift not found with contactId:{contactId}");
                else
                    return response.SetResponse(true, contact);
            }
            catch (Exception ex)
            {
                return response.SetResponseWithEx(ex.Message);
            }
        }

        public async Task<ServiceResponse<Contact>> AddContact(Contact contact)
        {
            var response = new ServiceResponse<Contact>();
            try
            {
                contact.ContactId = Guid.NewGuid();
                await _contacts.InsertOneAsync(contact);
                return response.SetResponse(true, "Gift created successfully", contact);
            }
            catch (Exception ex)
            {
                return response.SetResponseWithEx(ex.Message);
            }
        }

        public async Task<ServiceResponse<Contact>> UpdateContact(string contactId, Contact contact)
        {
            var response = new ServiceResponse<Contact>();
            try
            {
                var update = Builders<Contact>.Update.Set(u => u.Contacts.Value, contact.Contacts.Value);
                var updateResult = await _contacts.UpdateOneAsync(u => u.ContactId.ToString() == contactId, update);
                if (updateResult.ModifiedCount > 0)
                    return response.SetResponse(true, "Updated Contact details successfully");
                else
                    return response.SetResponse(false, "Contact not found or no changes made.");
            }
            catch (Exception ex)
            {
                return response.SetResponseWithEx(ex.Message);
            }
        }

        public async Task<ServiceResponse<bool>> DeleteContact(string contactId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var deleteResult = await _contacts.DeleteOneAsync(contact => contact.ContactId.ToString() == contactId);
                if (deleteResult.DeletedCount > 0)
                    return response.SetResponse(true, "Contact deleted successfully");
                else
                    return response.SetResponse(false, "Contact not found");
            }
            catch (Exception ex)
            {
                return response.SetResponseWithEx(ex.Message);
            }
        }
    }
}
