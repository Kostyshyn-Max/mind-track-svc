using MaksymKostyshyn.MindTrack.Data.Abstractions.Repositories;
using MaksymKostyshyn.MindTrack.Domain;
using MaksymKostyshyn.MindTrack.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace MaksymKostyshyn.MindTrack.Data.ADO.Repositories;

public class AchievementRepository(IDbConnectionFactory dbConnectionFactory, ILogger<AchievementRepository> logger) : IAchievementRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;
    private readonly ILogger<AchievementRepository> _logger = logger;

    public async Task<Achievement?> CreateAchievementAsync(Achievement achievement, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = _dbConnectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            await using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Achievement (Id, Name, Description, PointsReward) VALUES (@Id, @Name, @Description, @PointsReward)";
            command.Parameters.AddWithValue("@Id", achievement.Id);
            command.Parameters.AddWithValue("@Name", achievement.Name);
            command.Parameters.AddWithValue("@Description", achievement.Description);
            command.Parameters.AddWithValue("@PointsReward", achievement.PointsReward);

            int rowsAffected = await command.ExecuteNonQueryAsync(cancellationToken);

            if (rowsAffected >= 1)
            {
                return achievement;
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to create achievement.");
            return null;
        }
    }

    public async Task<bool> DeleteAchievementAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = _dbConnectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            await using var guardCheckCommand = connection.CreateCommand();
            guardCheckCommand.CommandText = "SELECT COUNT(*) FROM UserAchievement WHERE AchievementId = @AchievementId";
            guardCheckCommand.Parameters.AddWithValue("@AchievementId", id);
            
            var unlockedCount = (int)await guardCheckCommand.ExecuteScalarAsync(cancellationToken);
            if (unlockedCount > 0)
            {
                throw new InvalidOperationException($"Cannot delete achievement as it is has been unlocked by {unlockedCount} users.");               
            }

            await using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Achievement WHERE Id = @AchievementId";
            command.Parameters.AddWithValue("@AchievementId", id);

            var rowsAffected = await command.ExecuteNonQueryAsync(cancellationToken);
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to delete achievement {AchievementId}.", id);
            return false;
        }
    }

    public async Task<Achievement?> GetAchievementByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = _dbConnectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Name, Description, PointsReward FROM Achievement WHERE Id = @AchievementId";
            command.Parameters.AddWithValue("@AchievementId", id);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            if (!await reader.ReadAsync(cancellationToken))
            {
                return null;
            }

            return new Achievement(
                reader.GetGuid(reader.GetOrdinal("Id")),
                reader.GetString(reader.GetOrdinal("Name")),
                reader.GetString(reader.GetOrdinal("Description")),
                reader.GetInt32(reader.GetOrdinal("PointsReward"))
            );

        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to retrieve achievement {AchievementId}", id);
            return null;
        }
    }

    public async Task<AchievementUnlockResult?> UnlockAchievementAsync(Guid achievementId, Guid userId, CancellationToken cancellationToken)
    {
        await using var connection = _dbConnectionFactory.CreateConnection();
        SqlTransaction? transaction = null;

        try
        {
            await connection.OpenAsync(cancellationToken);
            transaction = (SqlTransaction)await connection.BeginTransactionAsync(cancellationToken);

            int pointsReward;

            await using (var getAchievementPointsCommand = new SqlCommand(@"SELECT PointsReward FROM Achievement WHERE Id = @AchievementId", connection, transaction))
            {
                getAchievementPointsCommand.Parameters.AddWithValue("@AchievementId", achievementId);

                var result = await getAchievementPointsCommand.ExecuteScalarAsync(cancellationToken) ?? throw new InvalidOperationException($"Achievement {achievementId} does not exist.");
                pointsReward = (int)result;
            }

            await using (var checkUnlockedAchievementCommand = new SqlCommand(@"SELECT 1 FROM UserAchievement WHERE PatientId = @PatientId AND AchievementId = @AchievementId", connection, transaction))
            {
                checkUnlockedAchievementCommand.Parameters.AddWithValue("@PatientId", userId);
                checkUnlockedAchievementCommand.Parameters.AddWithValue("@AchievementId", achievementId);

                var exists = await checkUnlockedAchievementCommand.ExecuteScalarAsync(cancellationToken);
                if (exists is not null)
                {
                    throw new InvalidOperationException($"This achievement is already unlocked by user.");
                }
            }

            await using (var unlockAchievementCommand = new SqlCommand(@"INSERT INTO UserAchievement (Id, PatientId, AchievementId, UnlockedAt) VALUES (@Id, @PatientId, @AchievementId, @UnlockedAt)", connection, transaction))
            {
                unlockAchievementCommand.Parameters.AddWithValue("@Id", Guid.NewGuid());
                unlockAchievementCommand.Parameters.AddWithValue("@PatientId", userId);
                unlockAchievementCommand.Parameters.AddWithValue("@AchievementId", achievementId);
                unlockAchievementCommand.Parameters.AddWithValue("@UnlockedAt", DateTime.UtcNow);

                await unlockAchievementCommand.ExecuteNonQueryAsync(cancellationToken);
            }

            int newTotalPoints;
            await using (var updatePatientTotalPointsCommand = new SqlCommand(@"UPDATE [User] SET EarnedPoints = EarnedPoints + @PointsReward OUTPUT INSERTED.EarnedPoints WHERE Id = @UserId", connection, transaction))
            {
                updatePatientTotalPointsCommand.Parameters.AddWithValue("@PointsReward", pointsReward);
                updatePatientTotalPointsCommand.Parameters.AddWithValue("@UserId", userId);

                var updatedPoints = await updatePatientTotalPointsCommand.ExecuteScalarAsync(cancellationToken);
                if (updatedPoints is null)
                {
                    throw new InvalidOperationException("User does not found.");
                }

                newTotalPoints = (int)updatedPoints;
            }

            await transaction.CommitAsync(cancellationToken);

            return new()
            {
                PointsAwarded = pointsReward,
                TotalUserPoints = newTotalPoints
            };
        }
        catch (Exception ex)
        {
            if (transaction is not null)
            {
                await transaction.RollbackAsync(cancellationToken);
            }

            _logger.LogWarning(ex, "Failed to unlock achievement {AchievementId} for user {UserId}.", achievementId, userId);
            return null;
        }
    }

    public async Task<bool> UpdateAchievementAsync(Achievement achievement, CancellationToken cancellationToken)
    {
        try
        {
            const string updateQuery = @"
                UPDATE Achievement
                SET Name = @Name,
                    Description = @Description,
                    PointsReward = @PointsReward
                WHERE Id = @AchievementId";

            await using var connection = _dbConnectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            await using var command = connection.CreateCommand();
            command.CommandText = updateQuery;
            command.Parameters.AddWithValue("@Name", achievement.Name);
            command.Parameters.AddWithValue("@Description", achievement.Description);
            command.Parameters.AddWithValue("@PointsReward", achievement.PointsReward);
            command.Parameters.AddWithValue("@AchievementId", achievement.Id);

            var rowsAffected = await command.ExecuteNonQueryAsync(cancellationToken);
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to update achievement {AchievementId}", achievement.Id);
            return false;
        }
    }
}
