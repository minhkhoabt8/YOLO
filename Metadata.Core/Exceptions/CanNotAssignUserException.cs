using SharedLib.Core.Exceptions;


namespace Metadata.Core.Exceptions
{
    public class CanNotAssignUserException : HandledException
    {
        /// <summary>
        /// if user token not have username
        /// </summary>
        public CanNotAssignUserException() : base(400, "Can Not Assign Creator To User")
        {
        }

    }

    public class CannotAssignSignerException : HandledException
    {
        /// <summary>
        /// If user not a signer role 
        /// </summary>
        public CannotAssignSignerException() : base(40, "Can Not Assign Signer To User")
        {
        }
    }
}
