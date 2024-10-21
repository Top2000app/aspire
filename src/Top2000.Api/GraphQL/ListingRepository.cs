using Dapper;
using Top2000.Api.Database;

namespace Top2000.Api.GraphQL;

public class ListingRepository
{
    private readonly IDbConnectionFactory dbConnectionFactory;

    public ListingRepository(IDbConnectionFactory dbConnectionFactory)
    {
        this.dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IEnumerable<TrackListing>> GetAllListingByEditions(int year)
    {
        var sql =
          "SELECT Listing.TrackId, Listing.Position, (p.Position - Listing.Position) AS Delta, Listing.PlayUtcDateAndTime, Title, Artist " +
          "FROM Listing JOIN Track ON Listing.TrackId = Id " +
          $"LEFT JOIN Listing as p ON p.TrackId = Id AND p.Edition = {year - 1} " +
          $"WHERE Listing.Edition = {year} " +
          "ORDER BY Listing.Position";

        var connection = await dbConnectionFactory.CreateConnectionAsync(CancellationToken.None);
        var result = await connection.QueryAsync<TrackListing>(sql);

        return result;
    }
}
