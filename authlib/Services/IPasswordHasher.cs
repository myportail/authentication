namespace Authlib.Services
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
    }
}