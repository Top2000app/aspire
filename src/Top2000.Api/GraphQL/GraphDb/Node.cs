namespace Top2000.Api.GraphQL.GraphDb;

public class Node
{
    public int Id { get; set; }
    public List<Edge> Edges { get; set; } = [];
    public Dictionary<string, object> Properties { get; set; } = [];
}

public class Edge
{
    public Node TrackNode { get; set; }
    public Node EditionNode { get; set; }
    public Dictionary<string, object> Properties { get; set; } = [];
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