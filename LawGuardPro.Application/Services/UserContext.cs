using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using LawGuardPro.Application.Interfaces;

namespace LawGuardPro.Application.Services;

public class UserContext : IUserContext
{
    private readonly HttpContext _httpContext;
    private readonly ClaimsPrincipal _user;

    public string? Email
    {
        get
        {
            return _user.Claims
                .FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
        }
    }

    public Guid? UserId
    {
        get
        {
            var userId = _user.Claims
                .FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;

            return new Guid(userId!);
        }
    }

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor.HttpContext ?? new DefaultHttpContext();
        _user = _httpContext.User;
    }
}
