using MaksymKostyshyn.MindTrack.Data.Abstractions.Repositories;
using MaksymKostyshyn.MindTrack.Data.EF.Mappers;
using MaksymKostyshyn.MindTrack.Domain;
using Microsoft.Extensions.Logging;

namespace MaksymKostyshyn.MindTrack.Data.EF.Repositories;

public class PracticeRepository(MindTrackDbContext dbContext, ILogger<PracticeRepository> logger) : IPracticeRepository
{
    private readonly MindTrackDbContext _dbContext = dbContext;
    private readonly ILogger<PracticeRepository> _logger = logger;

    public async Task<Practice?> CreatePracticeAsync(Practice practice, CancellationToken cancellationToken)
    {
        try
        {
            var entity = practice.ToEntity();

            await _dbContext.Practice.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity.ToDomain();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to create practice");
            return null;
        }
    }

    public async Task DeletePracticeAsync(Guid practiceId, CancellationToken cancellationToken)
    {
        try
        {
            var practiceEntity = await _dbContext.Practice.FindAsync(practiceId, cancellationToken);
            if (practiceEntity is null)
            {
                _logger.LogWarning("Practice with {PracticeId} was not found", practiceId);
                return;
            }

            practiceEntity.IsActive = false;

            _dbContext.Practice.Update(practiceEntity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to delete practice with id: {PracticeId}", practiceId);
        }
    }

    public async Task UpdatePracticeAsync(Practice practice, CancellationToken cancellationToken)
    {
        try
        {
            var entity = practice.ToEntity();

            _dbContext.Practice.Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to update practice with id: {PracticeId}", practice.Id);
        }
    }
}
