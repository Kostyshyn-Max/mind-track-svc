using MaksymKostyshyn.MindTrack.Domain;

namespace MaksymKostyshyn.MindTrack.Data.Abstractions.Repositories;

public interface IJournalEntryRepository
{
    public Task<JournalEntry?> CreateJournalEntryAsync(JournalEntry journalEntry, CancellationToken cancellationToken);

    public Task<JournalEntry?> GetByIdAsync(Guid journalEntryId, CancellationToken cancellationToken);

    public Task DeleteJournalEntryAsync(Guid journalEntryId, CancellationToken cancellationToken);

    public Task UpdateJournalEntryAsync(JournalEntry journalEntry, CancellationToken cancellationToken);
}
