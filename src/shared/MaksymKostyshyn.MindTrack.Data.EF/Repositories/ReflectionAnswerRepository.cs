using MaksymKostyshyn.MindTrack.Data.Abstractions.Repositories;
using MaksymKostyshyn.MindTrack.Data.EF.Mappers;
using MaksymKostyshyn.MindTrack.Domain;
using MaksymKostyshyn.MindTrack.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaksymKostyshyn.MindTrack.Data.EF.Repositories;

public class ReflectionAnswerRepository(MindTrackDbContext dbContext, ILogger<ReflectionAnswerRepository> logger) : IReflectionAnswerRepository
{
    private readonly MindTrackDbContext _dbContext = dbContext;
    private readonly ILogger<ReflectionAnswerRepository> _logger = logger;

    public async Task<ReflectionAnswer?> AddReflectionAnswer(ReflectionAnswer reflectionAnswer, CancellationToken cancellationToken)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var entity = reflectionAnswer.ToEntity();

            await _dbContext.ReflectionAnswers.AddAsync(entity, cancellationToken);

            var analysis = await _dbContext.EmotionAnalyses.FirstAsync(analysis => analysis.JournalEntryId == entity.JournalEntryId, cancellationToken);

            if (Enum.GetNames<UserEmotions>().Contains(analysis.DominantEmotion))
            {
                var journalEntry = await _dbContext.JournalEntry.FindAsync(entity.JournalEntryId, cancellationToken);
                if (journalEntry is null)
                {
                    _logger.LogWarning("Jounral entry not found for this record, rollbacking transaction.");

                    await transaction.RollbackAsync(cancellationToken);
                    return null;
                }

                var userEntity = await _dbContext.User.FindAsync(journalEntry.PatientId, cancellationToken);
                userEntity!.AIResponseStyleId = (int)AIResponseStyleType.Cautious;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return entity.ToDomain();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);

            _logger.LogWarning(ex, "Failed to add reflection answer");

            return null;
        }
    }
}
