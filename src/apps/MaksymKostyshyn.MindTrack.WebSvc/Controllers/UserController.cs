using MaksymKostyshyn.MindTrack.Application.UseCases.Users;
using MaksymKostyshyn.MindTrack.WebSvc.Mappers;
using MaksymKostyshyn.MindTrack.WebSvc.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace MaksymKostyshyn.MindTrack.WebSvc.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="request">The user's email, password, full name, role, and related settings.</param>
    /// <param name="useCase">Handles validation and creation.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The created user, or 400 if creation failed.</returns>
    [HttpPost]
    [ProducesResponseType<UserResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUserAsync(
        AddUserRequest request,
        [FromServices] AddUserUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.HandleAsync(request.ToCommand(), cancellationToken);

        return result is not null ? Created((string?)null!, result.ToResponse()) : BadRequest();
    }

    /// <summary>
    /// Retrieves a user by its id.
    /// </summary>
    /// <param name="id">The user's id.</param>
    /// <param name="useCase">Fetches the user.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The user, or 404 if no user exists with that id.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<UserResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserByIdAsync(
        Guid id,
        [FromServices] GetUserUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.HandleAsync(id, cancellationToken);

        return result is not null ? Ok(result.ToResponse()) : NotFound();
    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="id">The user's id.</param>
    /// <param name="request">The user's new email, password, full name, role, and related settings.</param>
    /// <param name="useCase">Handles validation and the update.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>204 on success, or 404 if no user exists with that id.</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUserAsync(
        Guid id,
        UpdateUserRequest request,
        [FromServices] UpdateUserUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.HandleAsync(request.ToCommand(id), cancellationToken);

        return result ? NoContent() : NotFound();
    }

    /// <summary>
    /// Deletes a user.
    /// </summary>
    /// <param name="id">The user's id.</param>
    /// <param name="useCase">Handles the deletion.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>204 on success, or 404 if no user exists with that id.</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUserAsync(
        Guid id,
        [FromServices] DeleteUserUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.HandleAsync(id, cancellationToken);

        return result ? NoContent() : NotFound();
    }
}
