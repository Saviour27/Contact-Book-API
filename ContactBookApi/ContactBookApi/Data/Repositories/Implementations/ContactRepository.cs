using ContactBookApi.Data.Repositories.Interfaces;
using ContactBookApi.Domain.DTOs;
using ContactBookApi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactBookApi.Data.Repositories.Implementations
{
    public class ContactRepository : IContactRepository
    {
        private readonly ContactDbContext _db;

        public ContactRepository(ContactDbContext db)
        {
            _db = db;
        }
        public async  Task<Contact> AddContactAsync(Contact contact)
        {
            var newContact = await _db.Contacts.AddAsync(contact);
            newContact.Entity.CreatedAt = DateTime.UtcNow;
            newContact.Entity.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return newContact.Entity;
        }

        public async Task<Contact> GetById(string Id)
        {
            var contact = await _db.Contacts.Include(x => x.AppUser).Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (contact == null)
            {
                throw new Exception("User does not exist");
            }
            return contact;
        }

        public async Task Delete(Contact contact)
        {
             _db.Contacts.Remove(contact);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Contact>> Get(PaginationFilter filter)
        {
            List<Contact> contactsToGet = new List<Contact>();
            var validPagesFilter = new PaginationFilter(filter.CurrentPage);

            var resp = await _db.Contacts.Include(x => x.Address)
                .Skip((validPagesFilter.CurrentPage - 1) * 4)
                .Take(4).ToListAsync();
            var num = resp.Count();

            for (int i = 0; i < num; i++)
            {
                Contact contactDto = new Contact()
                {
                    Name = resp[i].Name,
                    Email = resp[i].Email,
                    Phone = resp[i].Phone,
                    Address = resp[i].Address,
                    FacebookHandle = resp[i].FacebookHandle,
                    CreatedAt = resp[i].CreatedAt,
                    UpdatedAt = resp[i].UpdatedAt,
                    DeletedAt = resp[i].DeletedAt
                };
                contactsToGet.Add(contactDto);
            }
            return contactsToGet;           

        }

        public async Task<bool> Update(string Id, Contact contact)
        {
            var foundContact = await GetById(Id);
            foundContact.Name = contact.Name;
            foundContact.Email = contact.Email;
            foundContact.Address = contact.Address;
            foundContact.FacebookHandle = contact.FacebookHandle;
            foundContact.CreatedAt = contact.CreatedAt;
            foundContact.DeletedAt = contact.DeletedAt;
            foundContact.UpdatedAt = contact.UpdatedAt;

            _db.Update(foundContact);
            return await _db.SaveChangesAsync() > 0;
        }

        public IQueryable<Contact> Search(string name, string email, string address)
        {
            IQueryable<Contact> query = _db.Contacts.Include(x => x.Address);
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.Name == name || e.Name == name);
            }
            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(e => e.Email == email || e.Email == email);
            }
            if (!string.IsNullOrEmpty(address))
            {
                query = query.Where(e => e.Address == address || e.Address == address);
            }
            return query;
        }

        public async Task<Contact> GetByEmail(string email)
        {
            var contact = await _db.Contacts.Include(x => x.AppUser).Where(x => x.Email == email).FirstOrDefaultAsync();
            if (contact == null)
            {
                throw new Exception("User does not exist");
            }
            return contact;
        }

        public async Task<Contact> GetByIdOrEmail(string emailorId)
        {
            if (emailorId.Contains('@'))
            {
                var contact = await _db.Contacts.Include(x => x.AppUser).Where(x => x.Email == emailorId).SingleOrDefaultAsync();
                if (contact == null)
                {
                    throw new Exception("User does not exist");
                }
                return contact;
            }
            else
            {
                var contact = await _db.Contacts.Include(x => x.AppUser).Where(x => x.Id == emailorId).SingleOrDefaultAsync();

                if (contact == null)
                {
                    throw new Exception("User does not exist");
                }
                return contact;
            }
        }

        public async Task<List<Contact>> GetContactsByUser(string userId)
        {
            var contacts = await _db.Contacts.Where(c => c.AppUserId == userId).ToListAsync();
            
            return contacts;
        }

        public async Task<Contact> GetContactById(string contactId, string userId)
        {
            var contact = await _db.Contacts.FirstOrDefaultAsync(c => c.AppUserId == userId && c.Id == contactId);

            return contact;
        }

        public async Task<Contact> UpdateContact(Contact contact)
        {
            var newContact = _db.Contacts.Update(contact);
            newContact.Entity.UpdatedAt = DateTime.Now;
            await _db.SaveChangesAsync();

            
            return newContact.Entity;
        }
    }
}
