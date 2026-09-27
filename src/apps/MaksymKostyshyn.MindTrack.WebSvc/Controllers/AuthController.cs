using MaksymKostyshyn.MindTrack.Application.Commands;
using MaksymKostyshyn.MindTrack.Application.UseCases.Auth;
using MaksymKostyshyn.MindTrack.WebSvc.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace MaksymKostyshyn.MindTrack.WebSvc.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ExchangeTokenUseCase exchangeTokenUseCase) : ControllerBase
{
    private readonly ExchangeTokenUseCase _exchangeTokenUseCase = exchangeTokenUseCase;

    [HttpPost("token")]
    public async Task<IActionResult> ExchangeTokenAsync([FromForm] TokenExchangeRequest request, CancellationToken cancellationToken)
    {
        var command = new ExchangeTokenCommand(request.Code!, request.RedirectUri!, request.CodeVerifier!);

        var result = await _exchangeTokenUseCase.ExecuteAsync(command, cancellationToken);

        return Ok(TokenExchangeResponse.FromDomain(result));
    }
}
