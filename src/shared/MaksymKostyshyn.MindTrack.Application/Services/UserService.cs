using MaksymKostyshyn.MindTrack.Application.Abstractions.Services;
using MaksymKostyshyn.MindTrack.Data.Abstractions.Repositories;
using MaksymKostyshyn.MindTrack.Domain;
using System.Security.Cryptography;
using System.Text;

namespace MaksymKostyshyn.MindTrack.Application.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<User?> CreateUserAsync(string email, string password, string fullName, int roleId, Guid? therapistId, bool isVulnerable, int aiResponseStyleTypeId, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var passwordBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        var hashedPassword = Convert.ToHexString(passwordBytes);

        var user = new User(Guid.NewGuid(), email, hashedPassword, fullName, roleId, therapistId, isVulnerable, aiResponseStyleTypeId, 0, true, now, now);

        var result = await _userRepository.CreateUserAsync(user, cancellationToken);
        return result;
    }

    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _userRepository.GetByIdAsync(id, cancellationToken);
        return result;
    }

    public async Task<bool> UpdateUserAsync(Guid id, string email, string password, string fullName, int roleId, Guid? therapistId, bool isVulnerable, int aiResponseStyleTypeId, CancellationToken cancellationToken)
    {
        var existing = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
        {
            return false;
        }

        var user = new User(id, email, password, fullName, roleId, therapistId, isVulnerable, aiResponseStyleTypeId, existing.EarnedPoints, existing.IsActive, existing.CreatedAt, DateTime.UtcNow);

        await _userRepository.UpdateUserAsync(user, cancellationToken);
        return true;
    }

    public async Task<bool> DeleteUserAsync(Guid id, CancellationToken cancellationToken)
    {
        var existing = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
        {
            return false;
        }

        await _userRepository.DeleteUserAsync(id, cancellationToken);
        return true;
    }
}
