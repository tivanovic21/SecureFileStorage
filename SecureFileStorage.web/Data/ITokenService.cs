namespace SecureFileStorage.Web.Data
{
    public interface ITokenService
    {
        string GenerateToken(int userId);
    }
}