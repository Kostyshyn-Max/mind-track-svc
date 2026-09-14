using MaksymKostyshyn.MindTrack.Domain;
using MaksymKostyshyn.MindTrack.Domain.Models;

namespace MaksymKostyshyn.MindTrack.Data.Abstractions.Repositories;

public interface IAchievementRepository
{
    public Task<Achievement?> CreateAchievementAsync(Achievement achievement, CancellationToken cancellationToken);
    public Task<AchievementUnlockResult?> UnlockAchievementAsync(Guid achievementId, Guid userId, CancellationToken cancellationToken);
    public Task<bool> DeleteAchievementAsync(Guid id, CancellationToken cancellationToken);
    public Task<bool> UpdateAchievementAsync(Achievement achievement, CancellationToken cancellationToken);
    public Task<Achievement?> GetAchievementByIdAsync(Guid id, CancellationToken cancellationToken);
}
