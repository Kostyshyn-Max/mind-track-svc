using FluentValidation;
using MaksymKostyshyn.MindTrack.Application.Abstractions.Services;
using MaksymKostyshyn.MindTrack.Application.Commands;
using MaksymKostyshyn.MindTrack.Domain;

namespace MaksymKostyshyn.MindTrack.Application.UseCases.Users;

public class AddUserUseCase(IUserService userService, IValidator<AddUserCommand> validator)
{
    private readonly IUserService _userService = userService;
    private readonly IValidator<AddUserCommand> _validator = validator;

    public async Task<User?> HandleAsync(AddUserCommand command, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(command, cancellationToken);

        var result = await _userService.CreateUserAsync(command.Email, command.Password, command.FullName, command.RoleId, command.TherapistId, command.IsVulnerable, command.AIResponseStyleTypeId, cancellationToken);
        return result;
    }
}
