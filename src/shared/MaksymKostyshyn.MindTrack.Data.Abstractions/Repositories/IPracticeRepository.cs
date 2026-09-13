using MaksymKostyshyn.MindTrack.Domain;

namespace MaksymKostyshyn.MindTrack.Data.Abstractions.Repositories;

public interface IPracticeRepository
{
    public Task<Practice?> CreatePracticeAsync(Practice practice, CancellationToken cancellationToken);
    public Task UpdatePracticeAsync(Practice practice, CancellationToken cancellationToken);
    public Task DeletePracticeAsync(Guid practiceId, CancellationToken cancellationToken);
}
