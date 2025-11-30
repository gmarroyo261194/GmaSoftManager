namespace ManagerApi.DTOs.Auth;

public class TokenDto
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Expiration { get; set; }
}
