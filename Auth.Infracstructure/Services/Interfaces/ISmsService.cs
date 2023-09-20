using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.Services.Interfaces
{
    public interface ISmsService
    {
        Task SendSmsAsync(string phone, string otp);
    }
}
