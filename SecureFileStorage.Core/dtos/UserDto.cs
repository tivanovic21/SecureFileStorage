using SecureFileStorage.Core.Entities;

namespace SecureFileStorage.Core.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int UserTypeId { get; set; }
        public DateTime LastLogin { get; set; }

        public UserType UserType { get; set; }
    }
}