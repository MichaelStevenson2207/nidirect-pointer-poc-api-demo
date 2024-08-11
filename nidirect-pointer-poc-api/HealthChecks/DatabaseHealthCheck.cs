using System.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace nidirect_pointer_poc_api.HealthChecks;

/// <summary>
/// Health check for database
/// </summary>
public sealed class DatabaseHealthCheck : IHealthCheck
{
    private readonly string _connectionString;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="connectionString"></param>
    public DatabaseHealthCheck(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Checks the health of the database
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
    {
        try
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                // Perform custom performance check logic
                TimeSpan elapsed = stopwatch.Elapsed;
                if (elapsed.TotalMilliseconds > 600)
                {
                    return HealthCheckResult.Degraded("SQL Server response time is degraded");
                }

                return HealthCheckResult.Healthy();
            }
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(ex.ToString());
        }
    }
}