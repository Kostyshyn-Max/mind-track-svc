using MaksymKostyshyn.MindTrack.Data.Abstractions.Repositories;
using MaksymKostyshyn.MindTrack.Data.EF.Mappers;
using MaksymKostyshyn.MindTrack.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaksymKostyshyn.MindTrack.Data.EF.Repositories;

public class JournalEntryRepository(MindTrackDbContext dbContext, ILogger<JournalEntryRepository> logger) : IJournalEntryRepository
{
    private readonly MindTrackDbContext _dbContext = dbContext;
    private readonly ILogger<JournalEntryRepository> _logger = logger;

    public async Task<JournalEntry?> CreateJournalEntryAsync(JournalEntry journalEntry, CancellationToken cancellationToken)
    {
        try
        {
            var entity = journalEntry.ToEntity();

            await _dbContext.JournalEntry.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity.ToDomain();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to create journal entry");
            return null;
        }
    }

    public async Task DeleteJournalEntryAsync(Guid journalEntryId, CancellationToken cancellationToken)
    {
        try
        {
            var journalEntryEntity = await _dbContext.JournalEntry.FindAsync(journalEntryId, cancellationToken);
            if (journalEntryEntity is null)
            {
                _logger.LogWarning("Journal entry with {JournalEntryId} was not found", journalEntryId);
                return;
            }

            _dbContext.JournalEntry.Remove(journalEntryEntity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to delete journal entry with id: {JournalEntryId}", journalEntryId);
        }
    }

    public async Task<JournalEntry?> GetByIdAsync(Guid journalEntryId, CancellationToken cancellationToken)
    {
        try
        {
            var journalEntryEntity = await _dbContext.JournalEntry
                .Include(entry => entry.AssignedPractices)
                    .ThenInclude(assignedPractice => assignedPractice.Practice)
                .Include(entry => entry.EmotionAnalysis)
                .Include(entry => entry.ReflectionAnswers)
                .FirstOrDefaultAsync(entry => entry.Id == journalEntryId, cancellationToken);

            return journalEntryEntity?.ToDomain();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to retrieve journal entry with id: {JournalEntryId}", journalEntryId);
            return null;
        }
    }

    public async Task UpdateJournalEntryAsync(JournalEntry journalEntry, CancellationToken cancellationToken)
    {
        try
        {
            var entity = journalEntry.ToEntity();

            _dbContext.JournalEntry.Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to update journal entry with id: {JournalEntryId}", journalEntry.Id);
        }
    }
}
