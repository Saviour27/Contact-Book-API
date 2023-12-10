using ContactBookApi.Domain.DTOs;
using ContactBookApi.Domain.Generics;

namespace ContactBookApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<Result<List<AppUserDTO>>> GetUsersAsync();
    }
}
