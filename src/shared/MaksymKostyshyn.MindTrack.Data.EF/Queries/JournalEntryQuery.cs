using MaksymKostyshyn.MindTrack.Data.Abstractions.Queries;
using MaksymKostyshyn.MindTrack.Data.EF.Mappers;
using MaksymKostyshyn.MindTrack.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaksymKostyshyn.MindTrack.Data.EF.Queries;

public class JournalEntryQuery(MindTrackDbContext dbContext, ILogger<JournalEntryQuery> logger) : IJournalEntryQuery
{
    private readonly MindTrackDbContext _dbContext = dbContext;
    private readonly ILogger<JournalEntryQuery> _logger = logger;

    public async Task<IList<JournalEntry>?> GetJournalEntriesForPatientAsync(Guid patientId, int page, int pageSize, CancellationToken cancellationToken)
    {
        try
        {
            var journalEntries = await _dbContext.JournalEntry
                .Where(entry => entry.PatientId == patientId)
                .OrderByDescending(entry => entry.RecordedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return journalEntries.Select(entry => entry.ToDomain()).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to fetch journal entries for patient: {PatientId}", patientId);
            return null;
        }
    }
}
