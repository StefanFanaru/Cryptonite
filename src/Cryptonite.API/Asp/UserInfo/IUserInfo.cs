namespace Cryptonite.API.Asp.UserInfo
{
    public interface IUserInfo
    {
        string Id { get; }
        string FirstName { get; }
        string LastName { get; }
        string Email { get; }
        string Role { get; }
        string Name { get; }
    }
}