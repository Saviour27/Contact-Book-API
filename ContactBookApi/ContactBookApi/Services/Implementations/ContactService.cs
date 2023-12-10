using ContactBookApi.Data;
using ContactBookApi.Data.Repositories.Interfaces;
using ContactBookApi.Domain.DTOs;
using ContactBookApi.Domain.Entities;
using ContactBookApi.Domain.Generics;
using ContactBookApi.Services.Interfaces;

namespace ContactBookApi.Services.Implementations
{
    public class ContactService : IContactService
    {
        private readonly ContactDbContext _db;
        private readonly IContactRepository _contactRepository;

        public ContactService(ContactDbContext db, IContactRepository contactRepository)
        {
            _db = db;
            _contactRepository = contactRepository;
        }
        public async Task<Result<AddContactResponseDTO>> AddContact(AddContactRequestDTO requestDTO, string userId)
        {
            var result = new Result<AddContactResponseDTO>(); 
            try
            {
                var user = _db.AppUsers.FirstOrDefault(u => u.Id == userId);
                if (user == null) 
                { 
                    result.IsSuccess = false;
                    result.ErrorMessage = "User not found";
                    return result;
                }

                var newContact = new Contact
                {
                    Address = requestDTO.Address,
                    AppUserId = user.Id,
                    Email = requestDTO.Email,
                    FacebookHandle = requestDTO.FaceBookHandle,
                    Phone = requestDTO.PhoneNumber,
                    Name = requestDTO.Name
                };

                await _contactRepository.AddContactAsync(newContact);

                var contactResponse = new AddContactResponseDTO
                {
                    AddedBy = user.Name,
                    Email = newContact.Email,
                    FaceBookHandle = newContact.FacebookHandle,
                    Id = newContact.Id,
                    Name = newContact.Name,
                    PhoneNumber = newContact.Phone,
                };

                result.IsSuccess = true;
                result.Message = "Contact added successfully";
                result.Content = contactResponse;


            }
            catch (Exception ex)
            {
                result.IsSuccess= false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public async Task<Result<object>> DeleteContact(string contactId, string userId)
        {
            var result = new Result<object>(); 
            try
            {
                var user = _db.AppUsers.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    result.IsSuccess = false;
                    result.ErrorMessage = "User not found";
                    return result;
                }

                var contact = _db.Contacts.FirstOrDefault(u => u.Id == contactId);
                if (contact == null)
                {
                    result.IsSuccess = false;
                    result.ErrorMessage = "Contact not found";
                    return result;
                }

                if (contact.AppUserId != user.Id)
                {
                    result.IsSuccess = false;
                    result.ErrorMessage = "You cannot delete this contact";
                    return result;
                }

                await _contactRepository.Delete(contact);
                contact.DeletedAt = DateTime.Now;

                result.IsSuccess = true;
                result.Message = "Contact deleted successfully";
                result.Content = new { DeletedAt = contact.DeletedAt };


            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public async Task<Result<List<GetContactDTO>>> GetAllContacts(string userId)
        {
           var result = new Result<List<GetContactDTO>>();
            try
            {
                var user = _db.AppUsers.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    result.IsSuccess = false;
                    result.ErrorMessage = "User not found";
                    return result;
                }

                var contacts = await _contactRepository.GetContactsByUser(user.Id);

                if(contacts.Count() == 0)
                {
                    result.IsSuccess = false;
                    result.ErrorMessage = "Contacts not found";
                    return result;
                }

                var getContacts = new List<GetContactDTO>();

                foreach (var contact in contacts)
                {
                    var contactDTO = new GetContactDTO 
                    {
                        Id = contact.Id,
                        Email = contact.Email,
                        FaceBookHandle = contact.FacebookHandle,
                        Name = contact.Name,
                        PhoneNumber= contact.Phone
                    };   

                    getContacts.Add(contactDTO);
                }

                result.IsSuccess = true;
                result.Message = "Contacts retrieved successfully";
                result.Content = getContacts;


            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public async Task<Result<GetContactDTO>> GetContactById(string contactId, string userId)
        {
           var result = new Result<GetContactDTO>(); 
            try
            {
                var user = _db.AppUsers.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    result.IsSuccess = false;
                    result.ErrorMessage = "User not found";
                    return result;
                }

                var contact = await _contactRepository.GetContactById(contactId, user.Id);

                if (contact == null)
                {
                    result.IsSuccess = false;
                    result.ErrorMessage = "Contact not found";
                    return result;
                }

                var getContact = new GetContactDTO
                {
                    Id = contact.Id,
                    Name = contact.Name,
                    PhoneNumber = contact.Phone,
                    Email = contact.Email,
                    FaceBookHandle = contact.FacebookHandle
                };

                result.IsSuccess = true;
                result.Message = "Contact retrieved successfully";
                result.Content = getContact;

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public async Task<Result<UpdateContactDTO>> UpdateContact(string contactId, UpdateContactDTO requestDTO, string userId)
        {
            var result = new Result<UpdateContactDTO>(); 
            try
            {
                var user = _db.AppUsers.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    result.IsSuccess = false;
                    result.ErrorMessage = "User not found";
                    return result;
                }

                var contact = _db.Contacts.FirstOrDefault(u => u.Id == contactId);

                if (contact == null)
                {
                    result.IsSuccess = false;
                    result.ErrorMessage = "Contact not found";
                    return result;
                }

                contact.Email = requestDTO.Email ?? contact.Email;
                contact.Address = requestDTO.Address ?? contact.Address;
                contact.Phone = requestDTO.PhoneNumber ?? contact.Phone;
                contact.FacebookHandle = requestDTO.FaceBookHandle ?? contact.FacebookHandle;
                contact.Name = requestDTO.Name ?? contact.Name;

                var updateContact = await _contactRepository.UpdateContact(contact);

                var updateDTO = new UpdateContactDTO 
                { 
                    Name = updateContact.Name, 
                    Email = updateContact.Email, 
                    Address = updateContact.Address,
                    FaceBookHandle = updateContact.FacebookHandle,
                    PhoneNumber = updateContact.Phone
                };

                result.IsSuccess = true;
                result.Message = "Contact updated successfully";
                result.Content = updateDTO;


            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
    }
}
