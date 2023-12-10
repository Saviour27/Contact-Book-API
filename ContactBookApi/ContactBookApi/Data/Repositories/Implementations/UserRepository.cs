using ContactBookApi.Data.Repositories.Interfaces;
using ContactBookApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactBookApi.Data.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ContactDbContext _db;

        public UserRepository(ContactDbContext db)
        {
            _db = db;
        }
        public async Task<List<AppUser>> GetAllUsers()
        {
           var users = await _db.AppUsers.ToListAsync();

           return users;
        }
    }
}
