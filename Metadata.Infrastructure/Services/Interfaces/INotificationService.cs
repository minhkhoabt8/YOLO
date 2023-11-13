using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface INotificationService
    {
        Task<string> SendNotificationToUserAsync(string userId, string title, string body);
    }
}
