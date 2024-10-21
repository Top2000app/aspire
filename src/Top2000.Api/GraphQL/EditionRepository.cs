using Dapper;
using Top2000.Api.Database;

namespace Top2000.Api.GraphQL;

public class EditionRepository
{
    private readonly IDbConnectionFactory dbConnectionFactory;

    public EditionRepository(IDbConnectionFactory dbConnectionFactory)
    {
        this.dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IEnumerable<Edition>> AllEditionsAsync()
    {
        var connection = await dbConnectionFactory.CreateConnectionAsync(CancellationToken.None);

        var editions = await connection.QueryAsync<Edition>("select * from edition");

        return editions;
    }

    public async Task<Edition> LatestAsync()
    {
        var connection = await dbConnectionFactory.CreateConnectionAsync(CancellationToken.None);

        var editions = await connection.QueryFirstAsync<Edition>("select top 1 * from edition order by year desc");

        return editions;
    }

    public async Task<Edition?> CurrentAsync()
    {
        var connection = await dbConnectionFactory.CreateConnectionAsync(CancellationToken.None);

        var editions = await connection.QueryFirstAsync<Edition?>(
            $"select top 1 * from edition where StartUtcDateAndTime > '{DateTime.UtcNow}' and EndUtcDateAndTime < '{DateTime.UtcNow}' ");

        return editions;
    }

    public async Task<Edition?> GetByYear(int year)
    {
        var connection = await dbConnectionFactory.CreateConnectionAsync(CancellationToken.None);

        var edition = await connection.QueryFirstAsync<Edition?>(
            $"select * from edition where year = {year}");

        return edition;
    }

    public async Task<Edition> AddEditions(int year, bool hasValue)
    {
        var connection = await dbConnectionFactory.CreateConnectionAsync(CancellationToken.None);

        var edition = await connection.QueryFirstAsync<Edition?>(
           $"insert into edition value({year}, '{year}-12-25T23:00:00', '{year}-12-31T23:00:00', false");

        return edition;
    }

}
