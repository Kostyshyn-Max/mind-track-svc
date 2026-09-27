namespace MaksymKostyshyn.MindTrack.Application.Abstractions.Exceptions;

public class InactiveUserException : Exception
{
    public InactiveUserException(string email)
        : base($"Account '{email}' is disabled.") { }
}
