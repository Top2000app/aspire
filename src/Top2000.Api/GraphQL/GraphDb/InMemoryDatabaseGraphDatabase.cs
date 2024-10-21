using Dapper;
using Top2000.Api.Database;

namespace Top2000.Api.GraphQL.GraphDb;

public class InMemoryDatabaseGraphDatabase
{
    public List<Node> EditionNodes { get; private set; } = [];
    public List<Node> TrackNodes { get; private set; } = [];
    public List<Edge> Edges { get; private set; } = [];

    public async Task InitialiseAsync(IDbConnectionFactory dbConnectionFactory)
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync(CancellationToken.None);

        var tracks = await connection.QueryAsync<Track>("select * from track");
        var listings = await connection.QueryAsync<Listing>("select * from Listing");
        var editions = await connection.QueryAsync<Edition>("select * from edition");

        foreach (var track in tracks)
        {
            var node = new Node
            {
                Id = track.Id,
                Properties = new Dictionary<string, object>
                {
                    { nameof(track.Title) , track.Title},
                    { nameof(track.Artist) , track.Artist},
                    { nameof(track.RecordedYear) , track.RecordedYear}
                }
            };

            TrackNodes.Add(node);
        }

        foreach (var edition in editions)
        {
            var node = new Node
            {
                Id = edition.Year,
                Properties = new Dictionary<string, object>
                {
                    { nameof(edition.StartUtcDateAndTime) , edition.StartUtcDateAndTime},
                    { nameof(edition.EndUtcDateAndTime) , edition.EndUtcDateAndTime},
                    { nameof(edition.HasPlayDateAndTime) , edition.HasPlayDateAndTime},
                }
            };

            EditionNodes.Add(node);
        }

        foreach (var listing in listings)
        {
            var edge = new Edge
            {
                TrackNode = TrackNodes.First(x => x.Id == listing.TrackId),
                EditionNode = EditionNodes.First(x => x.Id == listing.Edition),
                Properties = new Dictionary<string, object>
                {
                    { nameof(listing.Position) , listing.Position},
                }
            };

            if (listing.PlayUtcDateAndTime.HasValue)
            {
                edge.Properties.Add(nameof(listing.PlayUtcDateAndTime), listing.PlayUtcDateAndTime.Value);
            }


            Edges.Add(edge);

            edge.TrackNode.Edges.Add(edge);
            edge.EditionNode.Edges.Add(edge);

        }
    }
}
