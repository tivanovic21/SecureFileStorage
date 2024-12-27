using SecureFileStorage.Core.Dtos;

namespace SecureFileStorage.Core.Interfaces
{
    public interface IUserTypeService
    {
        Task<IEnumerable<UserTypeDto>> GetUserTypes();
    }
}