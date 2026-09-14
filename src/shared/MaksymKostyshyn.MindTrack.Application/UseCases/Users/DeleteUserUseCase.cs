using MaksymKostyshyn.MindTrack.Application.Abstractions.Services;

namespace MaksymKostyshyn.MindTrack.Application.UseCases.Users;

public class DeleteUserUseCase(IUserService userService)
{
    private readonly IUserService _userService = userService;

    public async Task<bool> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _userService.DeleteUserAsync(id, cancellationToken);
        return result;
    }
}
