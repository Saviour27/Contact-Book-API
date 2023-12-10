using ContactBookApi.Data.Repositories.Interfaces;
using ContactBookApi.Domain.DTOs;
using ContactBookApi.Domain.Entities;
using ContactBookApi.Domain.Generics;
using ContactBookApi.Services.Implementations;
using ContactBookApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ContactBookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IContactRepository _contactRepository;

        public ContactController(IContactService contactService, SignInManager<AppUser> signInManager, IContactRepository contactRepository)
        {
            _contactService = contactService;
            _signInManager = signInManager;
            _contactRepository = contactRepository;

        }

        [HttpPost("add-contact")]
        [Authorize(Roles = "REGULAR")]
        public async Task<IActionResult> AddContact([FromBody] AddContactRequestDTO requestDTO)
        {
            var result = new Result<AddContactResponseDTO>();

            var user = await _signInManager.UserManager.GetUserAsync(User);

            result.RequestTime = DateTime.Now;

            var response = await _contactService.AddContact(requestDTO, user.Id);

            if (response.IsSuccess)
            {
                result.ResponseTime = DateTime.Now;
                result = response;
                return Ok(result);
            }

            result = response;
            result.ResponseTime = DateTime.Now;
            return BadRequest(result);
        }

        /// <summary>
        /// Only admin can delete a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{contactId}")]
        [Authorize(Roles = "REGULAR")]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string contactId)
        {
            var result = new Result<object>();

            var user = await _signInManager.UserManager.GetUserAsync(User);

            result.RequestTime = DateTime.Now;

            var response = await _contactService.DeleteContact(contactId, user.Id);

            if (response.IsSuccess)
            {
                result.ResponseTime = DateTime.Now;
                result = response;
                return Ok(result);
            }

            result = response;
            result.ResponseTime = DateTime.Now;
            return BadRequest(result);
        }

        /// <summary>
        /// This gets all Contact and can be conducted by only admin Users
        /// </summary>
        /// <returns></returns>
        [HttpGet("all-contacts")]
        [Authorize(Roles = "REGULAR")]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> GetContacts()
        {
            var result = new Result<List<GetContactDTO>>();

            var user = await _signInManager.UserManager.GetUserAsync(User);

            result.RequestTime = DateTime.Now;

            var response = await _contactService.GetAllContacts(user.Id);

            if (response.IsSuccess)
            {
                result.ResponseTime = DateTime.Now;
                result = response;
                return Ok(result);
            }

            result = response;
            result.ResponseTime = DateTime.Now;
            return BadRequest(result);

        }

        [HttpGet("get-contact/{contactId}")]
        [Authorize(Roles = "REGULAR")]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> GetContact(string contactId)
        {
            var result = new Result<GetContactDTO>();

            var user = await _signInManager.UserManager.GetUserAsync(User);

            result.RequestTime = DateTime.Now;

            var response = await _contactService.GetContactById(contactId, user.Id);

            if (response.IsSuccess)
            {
                result.ResponseTime = DateTime.Now;
                result = response;
                return Ok(result);
            }

            result = response;
            result.ResponseTime = DateTime.Now;
            return BadRequest(result);

        }

        //public async Task<IActionResult> GetContact(string contactId)
        //{
        //    var result = new Result<GetContactDTO>();

        //    var user = await _signInManager.UserManager.GetUserAsync(User);

        //    result.RequestTime = DateTime.Now;

        //    var response = await _contactService.GetContactById(contactId, user.Id);

        //    if (response.IsSuccess)
        //    {
        //        result.ResponseTime = DateTime.Now;
        //        result = response;
        //        return Ok(result);
        //    }

        //    result = response;
        //    result.ResponseTime = DateTime.Now;
        //    return BadRequest(result);

        //}

        [HttpPut("update-contact/{contactId}")]
        [Authorize(Roles = "REGULAR")]
        public async Task<IActionResult> UpdateContact(string contactId, UpdateContactDTO contactDTO)
        {
            var result = new Result<UpdateContactDTO>();

            var user = await _signInManager.UserManager.GetUserAsync(User);

            result.RequestTime = DateTime.Now;

            var response = await _contactService.UpdateContact(contactId, contactDTO, user.Id);

            if (response.IsSuccess)
            {
                result.ResponseTime = DateTime.Now;
                result = response;
                return Ok(result);
            }

            result = response;
            result.ResponseTime = DateTime.Now;
            return BadRequest(result);

        }

        /// <summary>
        /// Only admin can update other user's contact fields
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        [HttpPut("update/{Id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update(string Id, [FromForm] Contact contact)
        {
            var foundContact = _contactRepository.GetById(Id).GetAwaiter().GetResult();
            if (foundContact == null)
            {
                return NotFound("No such contact found");
            }

            var result = await _contactRepository.Update(Id, contact);

            if (result)
            {
                return Ok("Contact Successfully Updated");
            }
            else
            {
                return BadRequest("Something went wrong, try again");
            }
        }

        /// <summary>
        /// all logged in user can search for an existing user by name, city or state
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("search")]
        [Authorize(Roles = "admin,regular")]
        public IActionResult Search([FromQuery] SearchDTO model)
        {
            var contactToReturn = _contactRepository.Search(model.Name, model.Email, model.Address);
            if (contactToReturn == null)
            {
                return NotFound("No Contact associated search ");
            }
            var output = new List<SearchResponseDTO>();
            foreach (var contact in contactToReturn)
            {
                var response = new SearchResponseDTO
                {
                    Name = $"{contact.Name} {contact.Name}",
                    Email = $"{contact.Email} {contact.Email}",
                    Address = $"{contact.Address} {contact.Address}"
                };
                output.Add(response);
            }
            return Ok(output);
        }

        /// <summary>
        /// This gets contacts by Id or by Email and anyone can perform this but must be a logged in user
        /// </summary>
        /// <param name="emailOrId"></param>
        /// <returns></returns>
        [HttpGet("{emailOrId}")]
        [Authorize(Roles = "admin,regular")]
        public async Task<IActionResult> GetByIdOrEmail(string emailOrId)
        {
            return Ok(await _contactRepository.GetByIdOrEmail(emailOrId));
        }
    }
}
