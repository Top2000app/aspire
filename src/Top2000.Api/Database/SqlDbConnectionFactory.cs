using System.Data;
using Microsoft.Data.SqlClient;

namespace Top2000.Api.Database;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken);
}

public class SqlDbConnectionFactory : IDbConnectionFactory
{
    private readonly string connectionString;

    public SqlDbConnectionFactory(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        return connection;
    }
}
