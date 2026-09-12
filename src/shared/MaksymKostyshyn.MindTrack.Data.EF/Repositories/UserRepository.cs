using MaksymKostyshyn.MindTrack.Data.Abstractions.Repositories;
using MaksymKostyshyn.MindTrack.Data.EF.Mappers;
using MaksymKostyshyn.MindTrack.Domain;
using Microsoft.Extensions.Logging;

namespace MaksymKostyshyn.MindTrack.Data.EF.Repositories;

public class UserRepository(MindTrackDbContext dbContext, ILogger<UserRepository> logger) : IUserRepository
{
    private readonly MindTrackDbContext _dbContext = dbContext;
    private readonly ILogger<UserRepository> _logger = logger;

    public async Task<User?> CreateUserAsync(User user, CancellationToken cancellationToken)
    {
        try
        {
            var entity = user.ToEnity();

            await _dbContext.User.AddAsync(entity, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity.ToDomain();
        } 
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to create a new user");
            return null;
        }
    }

    public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            var userEntity = await _dbContext.User.FindAsync(userId, cancellationToken);
            if (userEntity is null)
            {
                _logger.LogWarning("User with {UserId} was not found", userId);
                return;
            }

            userEntity.IsActive = false;

            _dbContext.User.Update(userEntity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to delete user with id: {UserId}", userId);
        }
    }

    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            var userEntity = await _dbContext.User.FindAsync(userId, cancellationToken);
            return userEntity?.ToDomain();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to retrieve user with id: {UserId}", userId);
            return null;
        }
    }

    public async Task UpdateUserAsync(User user, CancellationToken cancellationToken)
    {
        try
        {
            var entity = user.ToEnity();

            _dbContext.User.Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        } 
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to update user with id: {UserId}", user.Id);
        }
    }
}
