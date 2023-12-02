using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.Services.Interfaces
{
    public interface ISmsService
    {
        Task SendOtpSmsAsync(string phone, string otp);

        Task SendPasswordSmsAsync(string phone, string password);

        Task SendPasswordEmail(string email, string password);

        Task SendOtpEmail(string email, string otp);
    }
}
