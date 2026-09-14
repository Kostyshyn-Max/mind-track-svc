using MaksymKostyshyn.MindTrack.Domain;

namespace MaksymKostyshyn.MindTrack.Data.Abstractions.Repositories;

public interface IAlertRepository
{
    public Task<Alert?> CreateAlertAsync(Alert alert, CancellationToken cancellationToken);
    public Task<Alert?> GetAlertByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task<bool> UpdateAlertAsync(Alert alert, CancellationToken cancellationToken);
    public Task<bool> DeleteAlertAsync(Guid id, CancellationToken cancellationToken);
}
