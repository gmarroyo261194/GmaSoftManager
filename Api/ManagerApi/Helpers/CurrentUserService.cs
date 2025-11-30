using System.Security.Claims;
using System.Linq;

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
        var httpContext = _httpContextAccessor?.HttpContext;
        if (httpContext == null)
            throw new System.Exception("HttpContext is not available.");

        var user = httpContext.User;
        // Si no hay usuario o no está autenticado, devolver Guid.Empty.
        // Esto permite flujos anónimos/intentos de crear el primer usuario sin fallar con excepción.
        if (user == null || user.Identity?.IsAuthenticated != true)
            return Guid.Empty;

        // Intent: buscar entre varios nombres de claim comunes
        var candidateClaimTypes = new[] {
            "UserId",
            System.Security.Claims.ClaimTypes.NameIdentifier,
            "sub",
            "user_id",
            "id",
            "uid"
        };

        var claim = user.Claims.FirstOrDefault(c =>
            candidateClaimTypes.Any(t => string.Equals(t, c.Type, StringComparison.OrdinalIgnoreCase)));

        if (claim == null)
        {
            var available = user.Claims.Select(c => c.Type).Distinct();
            var availableList = string.Join(", ", available);
            throw new System.Exception($"UserId claim not found. Buscados: {string.Join(", ", candidateClaimTypes)}. Claims presentes: {availableList}.");
        }

        if (!Guid.TryParse(claim.Value, out var userId))
        {
            throw new System.Exception($"UserId claim value tiene un formato inválido para GUID. ClaimType='{claim.Type}', Value='{claim.Value}'. Asegúrate de que el token incluya un GUID o adapta el parseo.");
        }

        return userId;
    }
}