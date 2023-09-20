namespace Auth.Infrastructure.DTOs.Authentication;

public class LoginOutputDTO
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string Token { get; set; }
    public int TokenExpires { get; set; }
    public string RefreshToken { get; set; }
    public int RefreshTokenExpires { get; set; }
    public string Role { get; set; }
}