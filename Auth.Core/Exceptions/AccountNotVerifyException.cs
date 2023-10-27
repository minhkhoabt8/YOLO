using SharedLib.Core.Exceptions;


namespace Auth.Core.Exceptions
{
    public class AccountNotVerifyException : HandledException
    {
        public AccountNotVerifyException() : base(403, "Account Is Not Verify")
        {
        }
    }
}
