using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Security
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _ctx;
        public UserAccessor(IHttpContextAccessor ctx)
        { 
            _ctx = ctx;            
        }
        public string GetUsername()
        {
            return _ctx.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        }
    }
}