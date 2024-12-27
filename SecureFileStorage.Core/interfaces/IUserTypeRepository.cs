using SecureFileStorage.Core.Entities;

namespace SecureFileStorage.Core.Interfaces
{
    public interface IUserTypeRepository
    {
        Task<IEnumerable<UserType>> GetUserTypesAsync();
    }
}