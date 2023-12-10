using ContactBookApi.Domain.DTOs;
using ContactBookApi.Domain.Generics;

namespace ContactBookApi.Services.Interfaces
{
    public interface IContactService
    {
        Task<Result<AddContactResponseDTO>> AddContact(AddContactRequestDTO requestDTO, string userId);

        Task<Result<object>> DeleteContact(string contactId, string userId);

        Task<Result<List<GetContactDTO>>> GetAllContacts(string userId);

        Task<Result<GetContactDTO>> GetContactById(string contactId, string userId);

        Task<Result<UpdateContactDTO>> UpdateContact(string contactId, UpdateContactDTO requestDTO, string userId);

    }
}
