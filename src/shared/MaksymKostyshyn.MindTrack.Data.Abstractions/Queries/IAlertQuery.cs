using MaksymKostyshyn.MindTrack.Domain.Models;

namespace MaksymKostyshyn.MindTrack.Data.Abstractions.Queries;

public interface IAlertQuery
{
    public Task<IList<AlertSeverityReport>?> GetAlertSummaryForPatientAsync(Guid patientId, CancellationToken cancellationToken);
}
