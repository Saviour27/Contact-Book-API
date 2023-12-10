using ContactBookApi.Domain.DTOs;
using ContactBookApi.Domain.Generics;

namespace ContactBookApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Result<LoginResponseDTO>>Login(LoginRequestDTO loginRequestDTO);
        Task<Result<AppUserDTO>> Register(RegistrationRequestDTO requestDTO);
        Task<Result<bool>> AssignRole(string email, string roleName);
    }
}
