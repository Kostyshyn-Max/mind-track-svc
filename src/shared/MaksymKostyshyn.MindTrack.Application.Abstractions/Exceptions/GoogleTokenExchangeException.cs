namespace MaksymKostyshyn.MindTrack.Application.Abstractions.Exceptions;

public class GoogleTokenExchangeException : Exception
{
    public GoogleTokenExchangeException(string message)
        : base(message) { }
}
