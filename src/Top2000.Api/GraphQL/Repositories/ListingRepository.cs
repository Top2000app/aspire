using Top2000.Api.GraphQL.GraphDb;

namespace Top2000.Api.GraphQL.Repositories;

public class ListingRepository
{
    private readonly InMemoryDatabaseGraphDatabase database;

    public ListingRepository(InMemoryDatabaseGraphDatabase database)
    {
        this.database = database;
    }

    public IEnumerable<Listing> GetAllListingByEditions(int year)
    {
        return database.Edges
            .Where(x => x.EditionNode.Id == year)
            .Select(Transform)
            .OrderBy(x => x.Position)
            ;
    }

    private Listing Transform(Edge edge)
    {
        var listing = new Listing
        {
            EditionYear = edge.EditionNode.Id,
            Position = (int)edge.Properties["Position"],
            TrackId = edge.TrackNode.Id,
        };

        if (edge.Properties.ContainsKey("PlayUtcDateAndTime"))
        {
            listing.PlayUtcDateAndTime = (DateTime)edge.Properties["PlayUtcDateAndTime"];
        }

        return listing;
    }

    public IEnumerable<Listing> GetAllListingByTrack(int trackId)
    {
        return database.Edges
            .Where(x => x.TrackNode.Id == trackId)
            .Select(Transform)
            .OrderBy(x => x.Position);
    }

    //public async Task<IEnumerable<EditionPosition>> GetEditionsByTrackId(int trackId)
    //{
    //    var sql =
    //      "SELECT Edition.*, Listing.Position AS Position " +
    //      "FROM Listing JOIN Edition ON Listing.Edition = Edition.Year " +
    //      $"WHERE Listing.TrackId = {trackId} " +
    //      "ORDER BY Edition.Year";

    //    var connection = await dbConnectionFactory.CreateConnectionAsync(CancellationToken.None);
    //    var result = await connection.QueryAsync<EditionPosition>(sql);

    //    return result;

    //}
}
