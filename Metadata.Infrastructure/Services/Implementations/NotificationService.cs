using Metadata.Infrastructure.Services.Interfaces;
using SharedLib.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly IFireBaseNotificationService _fireBaseNotificationService;

        public NotificationService(IFireBaseNotificationService notificationService)
        {
            _fireBaseNotificationService = notificationService;
        }

        public async Task<string> SendNotificationToUserAsync(string userId, string title, string body)
        {
            return await _fireBaseNotificationService.SendNotificationToUserAsync(userId, title, body);
        }
    }
}
