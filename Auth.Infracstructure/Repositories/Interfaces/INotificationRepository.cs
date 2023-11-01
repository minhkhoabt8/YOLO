using Auth.Core.Entities;
using Auth.Infrastructure.DTOs.Account;
using Auth.Infrastructure.DTOs.Notification;
using SharedLib.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.Repositories.Interfaces
{
    public interface INotificationRepository : 
        IAddAsync<Notification>,
        IGetAllAsync<Notification>,
        IFindAsync<Notification>,
        IQueryAsync<Notification, NotificationQuery>
    {
    }
}
