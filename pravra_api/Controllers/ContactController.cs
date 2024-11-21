using Microsoft.AspNetCore.Mvc;
using pravra_api.Extensions;
using pravra_api.Models;
using pravra_api.Services;

namespace pravra_api.Controllers
{
    [ApiController]
    [Route("api/contacts")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            var contacts = await _contactService.GetAllContacts();
            return contacts.ToActionResult();
        }

        [HttpGet("getContactById/{contactId}")]
        public async Task<IActionResult> GetContactById(string contactId)
        {
            var contact = await _contactService.GetContactById(contactId);
            if (contact == null) return NotFound();
            return contact.ToActionResult();
        }

        [HttpPost("addContact")]
        public async Task<IActionResult> AddContact([FromBody] Contact contact)
        {
            var newContact = await _contactService.AddContact(contact);
            return newContact.ToActionResult();
        }

        [HttpPut("updateContact/{contactId}")]
        public async Task<IActionResult> UpdateContact(string contactId, [FromBody] Contact contact)
        {
            var response = await _contactService.UpdateContact(contactId, contact);
            return response.ToActionResult();
        }

        [HttpDelete("deleteContact/{contactId}")]
        public async Task<IActionResult> DeleteContact(string contactId)
        {
            var response = await _contactService.DeleteContact(contactId);
            return response.ToActionResult();
        }
    }
}
