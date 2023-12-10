using ContactBookApi.Domain.Entities;

namespace ContactBookApi.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(AppUser appUser, IEnumerable<string> roles);
    }
}
