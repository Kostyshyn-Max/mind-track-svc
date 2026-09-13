using MaksymKostyshyn.MindTrack.Data.Abstractions.Queries;
using MaksymKostyshyn.MindTrack.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaksymKostyshyn.MindTrack.Data.EF.Queries;

public class UserQuery(MindTrackDbContext dbContext, ILogger<UserQuery> logger) : IUserQuery
{
    private readonly MindTrackDbContext _dbContext = dbContext;
    private readonly ILogger<UserQuery> _logger = logger;

    public async Task<IList<UserSummary>?> GetPatientsFromTherapistAsync(Guid therapistId, int page, int pageSize, CancellationToken cancellationToken)
    {
        try
        {
            var patients = await _dbContext.User
                .Where(user => user.TherapistId.Equals(therapistId) && user.IsActive)
                .Include(user => user.AIResponseStyle)
                .Select(user => new UserSummary(user.Id, user.FullName, user.Email, user.AIResponseStyle.Name, user.EarnedPoints))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(user => user.FullName)
                .ToListAsync(cancellationToken);

            return patients;
        } 
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to fetch all users from therapist: {TherapistId}", therapistId);
            return null;
        }
    }
}
