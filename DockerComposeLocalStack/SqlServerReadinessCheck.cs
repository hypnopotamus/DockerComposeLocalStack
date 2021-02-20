using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DockerComposeLocalStack
{
    public class SqlServerReadinessCheck : IHealthCheck
    {
        private readonly IConfiguration _configuration;

        public SqlServerReadinessCheck(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<HealthCheckResult> CheckHealthAsync
        (
            HealthCheckContext context,
            CancellationToken cancellationToken = new CancellationToken()
        )
        {
            try
            {
                await using var connection = new SqlConnection(_configuration.GetConnectionString("Master"));
                await using var command = connection.CreateCommand();

                command.CommandType = CommandType.Text;
                command.CommandText = @"
IF DB_ID('Messages') IS NOT NULL
BEGIN
	IF EXISTS
	(
		SELECT 1
		FROM Messages.INFORMATION_SCHEMA.TABLES
		WHERE TABLE_NAME='Message'
		GROUP BY TABLE_NAME
	)
		SELECT CONVERT(BIT, 1);
	ELSE
		SELECT CONVERT(BIT, 0);
END
ELSE
	SELECT CONVERT(BIT, 0);
            ";

                await connection.OpenAsync(cancellationToken);
                var exists = await command.ExecuteScalarAsync(cancellationToken);

                return exists is bool e && e
                    ? HealthCheckResult.Healthy()
                    : HealthCheckResult.Degraded();
            }
            catch
            {
                return HealthCheckResult.Unhealthy();
            }
        }
    }
}