using System.Security.Claims;

namespace ManagerApi.Helpers;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetUserId()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null || httpContext.User == null)
            throw new Exception("No authenticated user.");

        // Suponiendo que el claim principal del id de usuario está en JwtRegisteredClaimNames.Sub o en un custom claim "userId".
        var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier) // Standard claim
                          ?? httpContext.User.FindFirst(ClaimTypes.Name)         // Sometimes used
                          ?? httpContext.User.FindFirst("sub")                   // JWT registered claim
                          ?? httpContext.User.FindFirst("userId");               // Custom claim

        if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            throw new Exception("UserId claim not found.");

        // Si es un GUID, lo convertimos correctamente
        if (Guid.TryParse(userIdClaim.Value, out Guid userId))
            return userId;
        
        throw new Exception("UserId in claim is not a valid GUID.");
    }
}