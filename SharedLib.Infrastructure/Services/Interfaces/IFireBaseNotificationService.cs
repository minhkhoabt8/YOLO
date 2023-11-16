using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib.Infrastructure.Services.Interfaces
{
    public interface IFireBaseNotificationService
    {
        Task<string> SendNotificationToUserAsync(string userId, string title, string body);
    }
}
