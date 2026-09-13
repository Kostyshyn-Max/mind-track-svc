using MaksymKostyshyn.MindTrack.Domain;

namespace MaksymKostyshyn.MindTrack.Data.Abstractions.Repositories;

public interface IAssignedPracticeRepository
{
    public Task<AssignedPractice?> CreateAssignedPracticeAsync(AssignedPractice assignedPractice, CancellationToken cancellationToken);
    public Task UpdateAssignedPracticeAsync(AssignedPractice assignedPractice, CancellationToken cancellationToken);
}
