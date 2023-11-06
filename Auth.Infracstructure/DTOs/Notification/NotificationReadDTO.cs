using SharedLib.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.DTOs.Notification
{
    public class NotificationReadDTO
    {
        public string Id { get; set; }

        public string UserId { get; set; } 

        public string SenderId {  get; set; }

        public string NotificationContent { get; set; } 

        public DateTime CreatedDate { get; set; }

        public bool IsRead { get; set; } 

        public bool IsDeleted { get; set; }

    }
}
