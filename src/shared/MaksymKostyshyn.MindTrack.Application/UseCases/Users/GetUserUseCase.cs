using MaksymKostyshyn.MindTrack.Application.Abstractions.Services;
using MaksymKostyshyn.MindTrack.Domain;

namespace MaksymKostyshyn.MindTrack.Application.UseCases.Users;

public class GetUserUseCase(IUserService userService)
{
    private readonly IUserService _userService = userService;

    public async Task<User?> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _userService.GetUserByIdAsync(id, cancellationToken);
        return result;
    }
}
