namespace SharedLib.Infrastructure.Services.Interfaces
{
    public interface IUserContextService
    {
        string? AccountID { get; }
        string? Username { get; }
        string? Email { get; }
        bool IsAuthenticated { get; }
    }
}