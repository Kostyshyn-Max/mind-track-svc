using MaksymKostyshyn.MindTrack.Application.UseCases.Achievements;
using MaksymKostyshyn.MindTrack.Domain;
using MaksymKostyshyn.MindTrack.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
    private bool TryGetUserId(out Guid userId)
    {
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        return Guid.TryParse(claim, out userId);
    }

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
    /// Retrieves a page of achievements.
    /// </summary>
    /// <param name="page">The page number to retrieve (1-based).</param>
    /// <param name="pageSize">The number of achievements per page.</param>
    /// <param name="excludeUnlocked">When true, achievements the signed-in user has already unlocked are left out and not counted.</param>
    /// <param name="useCase">Fetches the achievements.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The requested page of achievements with the total number of matching achievements, or 401 if the token has no user id.</returns>
    [HttpGet]
    [ProducesResponseType<PagedResult<Achievement>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllAchievementsAsync(
        [FromServices] GetAllAchievementsUseCase useCase,
        CancellationToken cancellationToken,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool excludeUnlocked = false)
    {
        Guid? excludeForUserId = null;

        if (excludeUnlocked)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized();
            }

            excludeForUserId = userId;
        }

        var result = await useCase.HandleAsync(page, pageSize, excludeForUserId, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Retrieves the achievements the signed-in user has unlocked, newest first.
    /// </summary>
    /// <param name="useCase">Fetches the unlocked achievements.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The unlocked achievements with their unlock dates (empty when none), or 401 if the token has no user id.</returns>
    [HttpGet("unlocked")]
    [ProducesResponseType<IList<UnlockedAchievement>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUnlockedAchievementsAsync(
        [FromServices] GetUnlockedAchievementsUseCase useCase,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var result = await useCase.HandleAsync(userId, cancellationToken);

        return Ok(result);
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
