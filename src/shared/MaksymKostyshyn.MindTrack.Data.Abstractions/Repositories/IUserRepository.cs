using MaksymKostyshyn.MindTrack.Domain;

namespace MaksymKostyshyn.MindTrack.Data.Abstractions.Repositories;

public interface IUserRepository
{
    public Task<User?> CreateUserAsync(User user, CancellationToken cancellationToken);

    public Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);

    public Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken);

    public Task UpdateUserAsync(User user, CancellationToken cancellationToken);
}
