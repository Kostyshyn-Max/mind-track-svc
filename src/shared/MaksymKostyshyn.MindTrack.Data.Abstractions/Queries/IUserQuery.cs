using MaksymKostyshyn.MindTrack.Domain;
using MaksymKostyshyn.MindTrack.Domain.Models;

namespace MaksymKostyshyn.MindTrack.Data.Abstractions.Queries;

public interface IUserQuery
{
    public Task<IList<UserSummary>?> GetPatientsFromTherapistAsync(Guid therapistId, int page, int pageSize, CancellationToken cancellationToken);
    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}
