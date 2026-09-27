using MaksymKostyshyn.MindTrack.WebSvc.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace MaksymKostyshyn.MindTrack.WebSvc.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("token")]
    public async Task<IActionResult> ExchangeTokenAsync([FromForm] TokenExchangeRequest request, CancellationToken cancellationToken)
    {
        return Ok();
    }
}
