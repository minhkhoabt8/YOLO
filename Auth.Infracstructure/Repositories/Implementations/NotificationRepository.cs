using Auth.Core.Data;
using Auth.Core.Entities;
using Auth.Infrastructure.DTOs.Account;
using Auth.Infrastructure.DTOs.Notification;
using Auth.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Quartz.Util;
using SharedLib.Infrastructure.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.Repositories.Implementations
{
    public class NotificationRepository : GenericRepository<Notification, YoloAuthContext>, INotificationRepository
    {
        public NotificationRepository(YoloAuthContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Notification>> QueryAsync(NotificationQuery query, bool trackChanges = false)
        {
            IQueryable<Notification> notifications = _context.Notifications
                .Include(c => c.User)
                .Where(n => n.IsDeleted == false && n.UserId == query.AccountId)
                .OrderBy( n => n.IsRead == true)
                .ThenByDescending(n=>n.CreatedDate);

            if (!trackChanges)
            {
                notifications = notifications.AsNoTracking();
            }
            
            IEnumerable<Notification> enumeratedNotifications = notifications.AsEnumerable();

            return await Task.FromResult(enumeratedNotifications);
        }
    }
}
