using MaksymKostyshyn.MindTrack.Domain;

namespace MaksymKostyshyn.MindTrack.Application.Abstractions.Services;

public interface IUserService
{
    public Task<User?> CreateUserAsync(string email, string password, string fullName, int roleId, Guid? therapistId, bool isVulnerable, int aiResponseStyleTypeId, CancellationToken cancellationToken);
    public Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task<bool> UpdateUserAsync(Guid id, string email, string password, string fullName, int roleId, Guid? therapistId, bool isVulnerable, int aiResponseStyleTypeId, CancellationToken cancellationToken);
    public Task<bool> DeleteUserAsync(Guid id, CancellationToken cancellationToken);
}
