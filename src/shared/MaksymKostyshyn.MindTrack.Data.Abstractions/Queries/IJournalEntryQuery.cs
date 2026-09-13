using MaksymKostyshyn.MindTrack.Domain;

namespace MaksymKostyshyn.MindTrack.Data.Abstractions.Queries;

public interface IJournalEntryQuery
{
    public Task<IList<JournalEntry>?> GetJournalEntriesForPatientAsync(Guid patientId, int page, int pageSize, CancellationToken cancellationToken);
}
