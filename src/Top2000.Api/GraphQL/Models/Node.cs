using Dapper;
using Top2000.Api.Database;

namespace Top2000.Api.GraphQL.Models;

public class Node
{
    public string Id { get; set; }
    public List<string> EdgeIds { get; set; } = [];
    public Dictionary<string, object> Properties { get; set; } = [];
}

public class Edge
{
    public string Id { get; set; }
    public string NodeIdLeft { get; set; }
    public string NodeIdRight { get; set; }
    public Dictionary<string, object> Properties { get; set; } = [];
}

public class InMemoryDatabase
{
    public List<Node> Nodes { get; set; } = [];
    public List<Edge> Edges { get; set; } = [];

    public async Task InitialiseAsync(IDbConnectionFactory dbConnectionFactory)
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync(CancellationToken.None);

        var tracks = await connection.QueryAsync<Track>("select * from track");
        var listings = await connection.QueryAsync<Listing>("select * from Listing");
        var editions = await connection.QueryAsync<Edition>("select * from editions");

        List<Node> nodes = [];
        foreach (var track in tracks)
        {
            var node = new Node
            {
                Id = $"t{track.Id}",
                Properties = new Dictionary<string, object>
                {
                    { nameof(track.Title) , track.Title},
                    { nameof(track.Artist) , track.Artist},
                    { nameof(track.RecordedYear) , track.RecordedYear.ToString()}
                }
            };

            nodes.Add(node);
        }

        foreach (var edition in editions)
        {
            var node = new Node
            {
                Id = $"e{edition.Year}",
                Properties = new Dictionary<string, object>
                {
                    { nameof(edition.StartUtcDateAndTime) , edition.StartUtcDateAndTime},
                    { nameof(edition.EndUtcDateAndTime) , edition.EndUtcDateAndTime},
                    { nameof(edition.HasPlayDateAndTime) , edition.HasPlayDateAndTime},
                }
            };

            nodes.Add(node);
        }

        List<Edge> edges = [];
        foreach (var listing in listings)
        {
            var edge = new Edge
            {
                Id = $"t{listing.TrackId}_e{listing.Edition}",
                NodeIdLeft = $"t{listing.TrackId}",
                NodeIdRight = $"e{listing.Edition}",
                Properties = new Dictionary<string, object>
                {
                    { nameof(listing.Position) , listing.Position},
                    { nameof(listing.PlayUtcDateAndTime) , listing.PlayUtcDateAndTime.ToString() ?? "" },
                }
            };

            edges.Add(edge);

            var leftNode = nodes.First(x => x.Id == edge.NodeIdLeft);
            leftNode.EdgeIds.Add(edge.Id);

            var rightNode = nodes.First(x => x.Id == edge.NodeIdRight);
            rightNode.EdgeIds.Add(edge.Id);
        }
    }
}

public class Track
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public int RecordedYear { get; set; }
}

public class Listing
{
    public int TrackId { get; set; }
    public int Edition { get; set; }
    public int Position { get; set; }
    public DateTime? PlayUtcDateAndTime { get; set; }
}

public class Edition
{
    public int Year { get; set; }
    public DateTime StartUtcDateAndTime { get; set; }
    public DateTime EndUtcDateAndTime { get; set; }
    public bool HasPlayDateAndTime { get; set; }
}