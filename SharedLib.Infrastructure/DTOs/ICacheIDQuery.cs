namespace SharedLib.Infrastructure.DTOs;

public interface ICacheIDQuery
{
    // Used to avoid cached responses
    public string CacheID { get; set; }
}