using MaksymKostyshyn.MindTrack.Data.Abstractions.Repositories;
using MaksymKostyshyn.MindTrack.Data.EF.Mappers;
using MaksymKostyshyn.MindTrack.Domain;
using Microsoft.Extensions.Logging;

namespace MaksymKostyshyn.MindTrack.Data.EF.Repositories;

public class AssignedPracticeRepository(MindTrackDbContext dbContext, ILogger<AssignedPracticeRepository> logger) : IAssignedPracticeRepository
{
    private readonly MindTrackDbContext _dbContext = dbContext;
    private readonly ILogger<AssignedPracticeRepository> _logger = logger;

    public async Task<AssignedPractice?> CreateAssignedPracticeAsync(AssignedPractice assignedPractice, CancellationToken cancellationToken)
    {
        try
        {
            var entity = assignedPractice.ToEntity();

            await _dbContext.AssignedPractices.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity.ToDomain();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to create assigned practice");
            return null;
        }
    }

    public async Task UpdateAssignedPracticeAsync(AssignedPractice assignedPractice, CancellationToken cancellationToken)
    {
        try
        {
            var entity = assignedPractice.ToEntity();

            _dbContext.AssignedPractices.Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to update assigned practice with id: {AssignedPracticeId}", assignedPractice.Id);
        }
    }
}
