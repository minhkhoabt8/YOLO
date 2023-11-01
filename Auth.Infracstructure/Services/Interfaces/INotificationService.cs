
using Auth.Infrastructure.DTOs.Notification;
using SharedLib.Infrastructure.DTOs;


namespace Auth.Infrastructure.Services.Interfaces
{
    public interface INotificationService
    {
        Task<NotificationReadDTO> CreateNotificationAsync(NotificationWriteDTO dto);
        Task<PaginatedResponse<NotificationReadDTO>> QueryNotification(NotificationQuery query);
        Task<NotificationReadDTO> MarkAsReadAsync(string notiId);
        Task DeleteNotificationAsync(string notiId);
    }
}
