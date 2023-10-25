using SharedLib.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Core.Entities
{
    public partial class LogError
    {
        public string ErrorId { get; set; } = Guid.NewGuid().ToString();
        public int StatusCode { get; set; }
        public string ErrorInfo {  get; set; }
        public string Type {  get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now.SetKindUtc();
        public bool IsDeleted { get; set; } = false;

        public LogError CreateLogError(int statusCode, string errorInfo, string type, string userId, string userName ) 
        {
            return new LogError
            {
                StatusCode = statusCode,
                ErrorInfo = errorInfo,
                Type = type,
                UserId = userId,
                UserName = userName
            };
        }

    }
}
