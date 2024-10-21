using Top2000.Api.GraphQL.GraphDb;

namespace Top2000.Api.GraphQL.Repositories;

public class EditionRepository
{

    private readonly InMemoryDatabaseGraphDatabase database;

    public EditionRepository(InMemoryDatabaseGraphDatabase database)
    {
        this.database = database;
    }

    public IEnumerable<Edition> AllEditions()
    {
        var editions = database.EditionNodes
            .OrderBy(x => x.Id)
            .Select(Transform);

        return editions;
    }

    private static Edition Transform(Node node) => new()
    {
        EndUtcDateAndTime = (DateTime)node.Properties["EndUtcDateAndTime"],
        StartUtcDateAndTime = (DateTime)node.Properties["StartUtcDateAndTime"],
        HasPlayDateAndTime = (bool)node.Properties["HasPlayDateAndTime"],
        Year = node.Id
    };

    public Edition Latest()
    {
        return database.EditionNodes
            .OrderBy(x => x.Id)
            .Select(Transform)
            .Last();

    }

    public Edition? Current()
    {
        // just to make my life simpel... today has no current.
        // just don't run the tool in the last week of the year
        return null;
    }

    public Edition? GetByYear(int year)
    {
        var node = database.EditionNodes
             .FirstOrDefault(x => x.Id == year);

        if (node is null)
        {
            return null;
        }

        return Transform(node);
    }

    //public async Task<Edition> AddEditions(int year, bool hasValue)
    //{

    //    //var connection = await dbConnectionFactory.CreateConnectionAsync(CancellationToken.None);

    //    //var edition = await connection.QueryFirstAsync<Edition?>(
    //    //   $"insert into edition value({year}, '{year}-12-25T23:00:00', '{year}-12-31T23:00:00', false");

    //    //return edition;
    //}

}
