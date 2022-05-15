using Microsoft.AspNetCore.Http;

namespace Cryptonite.API.Asp.UserInfo
{
    public class AspUserInfo : UserInfo
    {
        public AspUserInfo(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor?.HttpContext?.User)
        {
        }
    }
}