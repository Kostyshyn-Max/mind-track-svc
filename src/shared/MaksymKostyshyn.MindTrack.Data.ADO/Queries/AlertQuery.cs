using MaksymKostyshyn.MindTrack.Data.Abstractions.Queries;
using MaksymKostyshyn.MindTrack.Domain.Models;
using Microsoft.Extensions.Logging;

namespace MaksymKostyshyn.MindTrack.Data.ADO.Queries;

public class AlertQuery(IDbConnectionFactory dbConnectionFactory, ILogger<AlertQuery> logger) : IAlertQuery
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;
    private readonly ILogger<AlertQuery> _logger = logger;

    public async Task<IList<AlertSeverityReport>?> GetAlertSummaryForPatientAsync(Guid patientId, CancellationToken cancellationToken)
    {
        try
        {
            const string reportQuery = @"
                SELECT 
                    al.Severity,
                    COUNT(*)            AS AlertCount,
                    MAX(al.DetectedAt)  AS MostRecentAlert,
                    SUM(CASE WHEN al.IsResolved = 0 THEN 1 ELSE 0 END) AS UnresolvedCount
                FROM Alert al
                WHERE al.PatientId = @PatientId
                GROUP BY al.Severity
                HAVING COUNT(*) > 0
                ORDER BY AlertCount DESC;";

            var results = new List<AlertSeverityReport>();

            await using var connection = _dbConnectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            await using var command = connection.CreateCommand();
            command.CommandText = reportQuery;
            command.Parameters.AddWithValue("@PatientId", patientId);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                results.Add(new AlertSeverityReport
                {
                    Severity = reader.GetString(reader.GetOrdinal("Severity")),
                    AlertCount = reader.GetInt32(reader.GetOrdinal("AlertCount")),
                    MostRecentAlert = reader.GetDateTime(reader.GetOrdinal("MostRecentAlert")),
                    UnresolvedCount = reader.GetInt32(reader.GetOrdinal("UnresolvedCount"))
                });
            }

            return results;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to generate alert report.");
            return null;
        }
    }
}
