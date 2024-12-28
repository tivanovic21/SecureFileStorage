using SecureFileStorage.Core.Dtos;
using SecureFileStorage.Core.Entities;
using SecureFileStorage.Core.Interfaces;

namespace SecureFileStorage.Infrastructure.Services
{
    public class UserService : IUserService 
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            return MapUserToDto(await _userRepository.GetUserByIdAsync(id));
        }

        private UserDto MapUserToDto(User user)
        {
            return new UserDto {
                Id = user.Id,
                Email = user.Email,
                LastLogin = user.LastLogin,
                UserTypeId = user.UserTypeId,
                UserType = user.UserType
            };
        }
    }
}