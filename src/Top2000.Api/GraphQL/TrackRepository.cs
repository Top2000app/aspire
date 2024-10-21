using Dapper;
using Top2000.Api.Database;
using Top2000.Api.GraphQL.Types;

namespace Top2000.Api.GraphQL;

public class TrackRepository
{
    private readonly IDbConnectionFactory dbConnectionFactory;

    public TrackRepository(IDbConnectionFactory dbConnectionFactory)
    {
        this.dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IEnumerable<Track>> SearchAsync(TrackSearchType trackSearch)
    {
        var sql =
            "SELECT * " +
            "FROM Track " +
            $"WHERE (Title LIKE '{trackSearch.Title ?? ""}') OR (Artist LIKE '{trackSearch.Artist ?? ""}')";

        var connection = await dbConnectionFactory.CreateConnectionAsync(CancellationToken.None);
        var result = await connection.QueryAsync<Track>(sql);

        return result;
    }
}
