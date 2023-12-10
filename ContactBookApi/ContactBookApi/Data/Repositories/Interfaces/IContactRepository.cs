using ContactBookApi.Domain.DTOs;
using ContactBookApi.Domain.Entities;

namespace ContactBookApi.Data.Repositories.Interfaces
{
    public interface IContactRepository
    {
        Task<Contact> AddContactAsync(Contact contact);
        Task Delete(Contact contact);
        Task<Contact> GetById(string id);
        Task<List<Contact>> Get(PaginationFilter filter);
        Task<bool> Update(string Id, Contact contact);
        IQueryable<Contact> Search(string name, string email, string address);
        Task<Contact> GetByIdOrEmail(string emailorId);
        Task<Contact> GetByEmail(string email);

        Task<List<Contact>> GetContactsByUser(string userId);

        Task<Contact> GetContactById(string contactId, string userId);

        Task<Contact> UpdateContact(Contact contact);
        //Task UpdatePhoto(int Id, string photoUrl);

    }
}
