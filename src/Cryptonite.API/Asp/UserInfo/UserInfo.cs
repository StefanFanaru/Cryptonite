using System.Linq;
using System.Security.Claims;

namespace Cryptonite.API.Asp.UserInfo
{
    public class UserInfo : IUserInfo
    {
        public UserInfo(ClaimsPrincipal user)
        {
            if (user != null)
            {
                Id = user.Claims.FirstOrDefault(x => x.Type == Claims.UserId)?.Value;
                FirstName = user.Claims.FirstOrDefault(x => x.Type == Claims.FirstName)?.Value;
                LastName = user.Claims.FirstOrDefault(x => x.Type == Claims.LastName)?.Value;
                Email = user.Claims.FirstOrDefault(x => x.Type == Claims.Email)?.Value;
                Role = user.Claims.FirstOrDefault(x => x.Type == Claims.Role)?.Value;
            }
        }

        public string Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string Role { get; }
        public string Name => $"{FirstName} {LastName}";
    }
}