using MaksymKostyshyn.MindTrack.Data.Abstractions.Queries;
using MaksymKostyshyn.MindTrack.Domain;
using MaksymKostyshyn.MindTrack.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace MaksymKostyshyn.MindTrack.Data.ADO.Queries;

public class AchievementQuery(IDbConnectionFactory dbConnectionFactory, ILogger<AchievementQuery> logger) : IAchievementQuery
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;
    private readonly ILogger<AchievementQuery> _logger = logger;

    public async Task<UserAchievementReport?> GetUnlockedAchievementsForUserAsync(Guid userId, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = _dbConnectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            await using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT
                    u.Id AS UserId,
                    u.FullName,
                    a.Id AS AchievementId,
                    a.Name AS AchievementName,
                    a.Description AS AchievementDescription,
                    a.PointsReward,
                    ua.UnlockedAt
                FROM UserAchievement ua
                INNER JOIN Achievement a ON a.Id = ua.AchievementId
                INNER JOIN [User] u ON u.Id = ua.PatientId
                WHERE ua.PatientId = @UserId
                  AND (@FromDate IS NULL OR ua.UnlockedAt >= @FromDate)
                  AND (@ToDate IS NULL OR ua.UnlockedAt <= @ToDate)
                ORDER BY ua.UnlockedAt DESC;";

            command.Parameters.Add(new SqlParameter("@UserId", SqlDbType.UniqueIdentifier) { Value = userId });
            command.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime2)
            {
                Value = (object?)fromDate ?? DBNull.Value
            });
            command.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.DateTime2)
            {
                Value = (object?)toDate ?? DBNull.Value
            });

            UserAchievementReport? result = null;

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                result ??= new UserAchievementReport
                {
                    UserId = reader.GetGuid(reader.GetOrdinal("UserId")),
                    UserFullName = reader.GetString(reader.GetOrdinal("FullName"))
                };

                result.UnlockedAchievements.Add(new Achievement(
                    reader.GetGuid(reader.GetOrdinal("AchievementId")),
                    reader.GetString(reader.GetOrdinal("AchievementName")),
                    reader.GetString(reader.GetOrdinal("AchievementDescription")),
                    reader.GetInt32(reader.GetOrdinal("PointsReward"))));
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to retrieve unlocked achievements for user: {UserId}", userId);
            return null;
        }
    }
}
