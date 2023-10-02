using SharedLib.Core.Exceptions;

namespace Auth.Core.Exceptions
{
    public class WrongCredentialsException : HandledException
    {
        public WrongCredentialsException() : base(401, "Username or Password is incorrect")
        {
        }
    }
}
