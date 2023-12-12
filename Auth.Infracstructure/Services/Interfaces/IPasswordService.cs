using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.Services.Interfaces
{
    public interface IPasswordService
    {
        bool ValidatePassword(string enteredPassword, string storedSalt, string storedHash);
        string GenerateHashPassword(string password);
    }
}
