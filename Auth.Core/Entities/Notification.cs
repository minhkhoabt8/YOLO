using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Entities
{
    public class Notification
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public string NotificationContent { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsRead { get; set; } = false;

        public bool IsDeleted { get; set; } = false;


        public Account Account { get; set; }
    }
}
