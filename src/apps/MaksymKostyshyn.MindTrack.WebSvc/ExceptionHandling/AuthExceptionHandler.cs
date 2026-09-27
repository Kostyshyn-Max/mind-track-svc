using MaksymKostyshyn.MindTrack.Application.Abstractions.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MaksymKostyshyn.MindTrack.WebSvc.ExceptionHandling;

public class AuthExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (status, title) = exception switch
        {
            UserNotRegisteredException => (StatusCodes.Status404NotFound, "User not registered"),
            InactiveUserException => (StatusCodes.Status403Forbidden, "User is inactive"),
            GoogleTokenExchangeException => (StatusCodes.Status400BadRequest, "Google token exchange failed"),
            _ => (0, string.Empty)
        };

        if (status == 0)
        {
            return false;
        }

        var problemDetails = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = exception.Message
        };

        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
