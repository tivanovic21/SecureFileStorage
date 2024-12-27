using SecureFileStorage.Core.Dtos;
using SecureFileStorage.Core.Entities;
using SecureFileStorage.Core.Interfaces;

namespace SecureFileStorage.Infrastructure.Services
{
    public class UserTypeService : IUserTypeService
    {
        private readonly IUserTypeRepository _userTypeRepository;

        public UserTypeService(IUserTypeRepository userTypeRepository)
        {
            _userTypeRepository = userTypeRepository;
        }

        public async Task<IEnumerable<UserTypeDto>> GetUserTypes()
        {
            var userTypes = await _userTypeRepository.GetUserTypesAsync();
            return userTypes.Select(MapTypeToDto).ToList();
        }

        private UserTypeDto MapTypeToDto(UserType type) {
            return new UserTypeDto {
                Id = type.Id,
                Name = type.Name
            };
        }
    }
}