using FluentValidation;
using MaksymKostyshyn.MindTrack.Application.Abstractions.Services;
using MaksymKostyshyn.MindTrack.Application.Commands;

namespace MaksymKostyshyn.MindTrack.Application.UseCases.Users;

public class UpdateUserUseCase(IUserService userService, IValidator<UpdateUserCommand> validator)
{
    private readonly IUserService _userService = userService;
    private readonly IValidator<UpdateUserCommand> _validator = validator;

    public async Task<bool> HandleAsync(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(command, cancellationToken);

        var result = await _userService.UpdateUserAsync(command.Id, command.Email, command.Password, command.FullName, command.RoleId, command.TherapistId, command.IsVulnerable, command.AIResponseStyleTypeId, cancellationToken);
        return result;
    }
}
