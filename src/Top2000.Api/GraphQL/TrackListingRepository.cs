using Dapper;
using Top2000.Api.Database;

namespace Top2000.Api.GraphQL;

public class TrackListingRepository
{
    private readonly IDbConnectionFactory dbConnectionFactory;

    public TrackListingRepository(IDbConnectionFactory dbConnectionFactory)
    {
        this.dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IEnumerable<TrackListing>> GetAllListingByEditions(int year)
    {
        var sql =
          "SELECT Listing.TrackId, Listing.Position, Listing.PlayUtcDateAndTime, Title, Artist " +
          "FROM Listing JOIN Track ON Listing.TrackId = Id " +
          $"WHERE Listing.Edition = {year} " +
          "ORDER BY Listing.Position";

        var connection = await dbConnectionFactory.CreateConnectionAsync(CancellationToken.None);
        var result = await connection.QueryAsync<TrackListing>(sql);

        return result;
    }

    public async Task<IEnumerable<EditionPosition>> GetEditionsByTrackId(int trackId)
    {
        var sql =
          "SELECT Edition.*, Listing.Position AS Position " +
          "FROM Listing JOIN Edition ON Listing.Edition = Edition.Year " +
          $"WHERE Listing.TrackId = {trackId} " +
          "ORDER BY Edition.Year";

        var connection = await dbConnectionFactory.CreateConnectionAsync(CancellationToken.None);
        var result = await connection.QueryAsync<EditionPosition>(sql);

        return result;

    }
}
