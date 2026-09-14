using MaksymKostyshyn.MindTrack.Data.Abstractions.Repositories;
using MaksymKostyshyn.MindTrack.Domain;
using Microsoft.Extensions.Logging;

namespace MaksymKostyshyn.MindTrack.Data.ADO.Repositories;

public class AlertRepository(IDbConnectionFactory dbConnectionFactory, ILogger<AlertRepository> logger) : IAlertRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;
    private readonly ILogger<AlertRepository> _logger = logger;

    public async Task<Alert?> CreateAlertAsync(Alert alert, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = _dbConnectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            await using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Alert (Id, PatientId, TherapistId, TriggerReason, Severity, DetectedAt, IsResolved)
                VALUES (@Id, @PatientId, @TherapistId, @TriggerReason, @Severity, @DetectedAt, @IsResolved)";
            command.Parameters.AddWithValue("@Id", alert.Id);
            command.Parameters.AddWithValue("@PatientId", alert.PatientId);
            command.Parameters.AddWithValue("@TherapistId", alert.TherapistId);
            command.Parameters.AddWithValue("@TriggerReason", alert.TriggerReason);
            command.Parameters.AddWithValue("@Severity", alert.Severity);
            command.Parameters.AddWithValue("@DetectedAt", alert.DetectedAt);
            command.Parameters.AddWithValue("@IsResolved", alert.IsResolved);

            var rowsAffected = await command.ExecuteNonQueryAsync(cancellationToken);

            if (rowsAffected >= 1)
            {
                return alert;
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to create alert.");
            return null;
        }
    }

    public async Task<Alert?> GetAlertByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = _dbConnectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            await using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Id, PatientId, TherapistId, TriggerReason, Severity, DetectedAt, IsResolved
                FROM Alert
                WHERE Id = @AlertId";
            command.Parameters.AddWithValue("@AlertId", id);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            if (!await reader.ReadAsync(cancellationToken))
            {
                return null;
            }

            return new Alert(
                reader.GetGuid(reader.GetOrdinal("Id")),
                reader.GetGuid(reader.GetOrdinal("PatientId")),
                reader.GetGuid(reader.GetOrdinal("TherapistId")),
                reader.GetString(reader.GetOrdinal("TriggerReason")),
                reader.GetString(reader.GetOrdinal("Severity")),
                reader.GetDateTime(reader.GetOrdinal("DetectedAt")),
                reader.GetBoolean(reader.GetOrdinal("IsResolved")));
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to retrieve alert {AlertId}", id);
            return null;
        }
    }

    public async Task<bool> UpdateAlertAsync(Alert alert, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = _dbConnectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            await using var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE Alert
                SET PatientId = @PatientId,
                    TherapistId = @TherapistId,
                    TriggerReason = @TriggerReason,
                    Severity = @Severity,
                    DetectedAt = @DetectedAt,
                    IsResolved = @IsResolved
                WHERE Id = @AlertId";
            command.Parameters.AddWithValue("@PatientId", alert.PatientId);
            command.Parameters.AddWithValue("@TherapistId", alert.TherapistId);
            command.Parameters.AddWithValue("@TriggerReason", alert.TriggerReason);
            command.Parameters.AddWithValue("@Severity", alert.Severity);
            command.Parameters.AddWithValue("@DetectedAt", alert.DetectedAt);
            command.Parameters.AddWithValue("@IsResolved", alert.IsResolved);
            command.Parameters.AddWithValue("@AlertId", alert.Id);

            var rowsAffected = await command.ExecuteNonQueryAsync(cancellationToken);
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to update alert {AlertId}", alert.Id);
            return false;
        }
    }

    public async Task<bool> DeleteAlertAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = _dbConnectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            await using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Alert WHERE Id = @AlertId";
            command.Parameters.AddWithValue("@AlertId", id);

            var rowsAffected = await command.ExecuteNonQueryAsync(cancellationToken);
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to delete alert {AlertId}.", id);
            return false;
        }
    }
}
