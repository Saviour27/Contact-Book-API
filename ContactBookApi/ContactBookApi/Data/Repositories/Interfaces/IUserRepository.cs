using ContactBookApi.Domain.Entities;

namespace ContactBookApi.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<AppUser>> GetAllUsers();
    }
}
