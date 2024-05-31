using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Services;

public interface ICurrentUser
{
    Guid UserId { get; }
    string UserName { get; }
    string Email { get; }
    string IPAddress { get; }
}

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId
    {
        get
        {
            Guid.TryParse(_httpContextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId);
            return userId;
        }
    }
    public string GivenName => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.GivenName);
    public string UserName => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
    public string Email => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
    public string IPAddress => _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
}
