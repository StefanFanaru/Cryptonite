using Cryptonite.API.Asp.UserInfo;

namespace Cryptonite.IntegrationTests.Authorization
{
    public class TestBasicUserInfo : IUserInfo
    {
        public string Id => TestConstants.UserId;
        public string FirstName => "Jhonny";
        public string LastName => "Test";
        public string Email => "jhonny.test@gmail.com";
        public string Name => $"{FirstName} {LastName}";
        public string Role => Roles.BasicUser;
    }
}