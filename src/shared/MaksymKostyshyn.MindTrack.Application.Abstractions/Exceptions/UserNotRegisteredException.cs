namespace MaksymKostyshyn.MindTrack.Application.Abstractions.Exceptions;

public class UserNotRegisteredException : Exception
{
    public UserNotRegisteredException(string email)
        : base($"No registered account for '{email}'.") { }
}
