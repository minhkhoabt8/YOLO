namespace SharedLib.Infrastructure.Services.Interfaces
{
    public interface IUserContextService
    {
        Guid? AccountID { get; }
        string? Username { get; }
        string? Email { get; }
        bool IsAuthenticated { get; }
    }
}