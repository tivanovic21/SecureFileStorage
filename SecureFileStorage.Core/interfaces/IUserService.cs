using SecureFileStorage.Core.Dtos;

namespace SecureFileStorage.Core.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(int id);
    }
}