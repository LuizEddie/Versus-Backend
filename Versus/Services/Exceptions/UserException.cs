namespace Versus.Services.Exceptions
{
    public class UserException : ApplicationException
    {
        public UserException(string message) : base (message) { }
    }
}
