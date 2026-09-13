using MaksymKostyshyn.MindTrack.Domain.Models;

namespace MaksymKostyshyn.MindTrack.Data.Abstractions.Queries;

public interface IPracticeQuery
{
    public Task<IList<GrouppedPractices>?> GetAllPracticesGrouppedByCategory(CancellationToken cancellationToken);
}
