using MaksymKostyshyn.MindTrack.Application.UseCases.Achievements;
using MaksymKostyshyn.MindTrack.Domain;
using MaksymKostyshyn.MindTrack.WebSvc.Mappers;
using MaksymKostyshyn.MindTrack.WebSvc.Models.Achievement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaksymKostyshyn.MindTrack.WebSvc.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AchievementController : ControllerBase
{
    /// <summary>
    /// Creates a new achievement.
    /// </summary>
    /// <param name="request">The achievement's name, description, and points reward.</param>
    /// <param name="useCase">Handles validation and creation.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The created achievement, or 400 if creation failed.</returns>
    [HttpPost]
    [ProducesResponseType<Achievement>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAchievementAsync(
        AddAchievementRequest request,
        [FromServices] AddAchievementUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.HandleAsync(request.ToCommand(), cancellationToken);

        return result is not null ? Created((string?)null!, result) : BadRequest();
    }

    /// <summary>
    /// Retrieves an achievement by its id.
    /// </summary>
    /// <param name="id">The achievement's id.</param>
    /// <param name="useCase">Fetches the achievement.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The achievement, or 404 if no achievement exists with that id.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<Achievement>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAchievementByIdAsync(
        Guid id,
        [FromServices] GetAchievementUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.HandleAsync(id, cancellationToken);

        return result is not null ? Ok(result) : NotFound();
    }

    /// <summary>
    /// Updates an existing achievement.
    /// </summary>
    /// <param name="id">The achievement's id.</param>
    /// <param name="request">The achievement's new name, description, and points reward.</param>
    /// <param name="useCase">Handles validation and the update.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>204 on success, or 404 if no achievement exists with that id.</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAchievementAsync(
        Guid id,
        UpdateAchievementRequest request,
        [FromServices] UpdateAchievementUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.HandleAsync(request.ToCommand(id), cancellationToken);

        return result ? NoContent() : NotFound();
    }

    /// <summary>
    /// Deletes an achievement.
    /// </summary>
    /// <param name="id">The achievement's id.</param>
    /// <param name="useCase">Handles the deletion.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>204 on success, or 404 if no achievement exists with that id.</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAchievementAsync(
        Guid id,
        [FromServices] DeleteAchievementUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.HandleAsync(id, cancellationToken);

        return result ? NoContent() : NotFound();
    }
}
