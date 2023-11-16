using SharedLib.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.DTOs.Notification
{
    public class NotificationQuery : PaginatedQuery
    {
        [Required]
        public string AccountId { get; set; }
    }
}
