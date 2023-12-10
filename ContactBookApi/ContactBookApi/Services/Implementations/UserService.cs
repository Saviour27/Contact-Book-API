using ContactBookApi.Data.Repositories.Interfaces;
using ContactBookApi.Domain.DTOs;
using ContactBookApi.Domain.Entities;
using ContactBookApi.Domain.Generics;
using ContactBookApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ContactBookApi.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;

        public UserService(IUserRepository userRepository, UserManager<AppUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }
        public async Task<Result<List<AppUserDTO>>> GetUsersAsync()
        {
            var result = new Result<List<AppUserDTO>>();

            try
            {
                var users = await _userRepository.GetAllUsers();

                if(users.Count ==  0)
                {
                    result.IsSuccess = false;
                    result.ErrorMessage = "No users found";
                    return result;
                }

                var getUsers = new List<AppUserDTO>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    var getUser = new AppUserDTO
                    {
                        Email = user.Email,
                        Id = user.Id,
                        Name = user.Name,
                        PhoneNumber = user.PhoneNumber,
                        RoleName = roles
                    };

                    getUsers.Add(getUser);
                }

                result.IsSuccess = true;
                result.Message = "Users retrieved successfully";
                result.Content = getUsers;
            }
            catch (Exception ex)
            {
                result.IsSuccess= false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
    }
}
