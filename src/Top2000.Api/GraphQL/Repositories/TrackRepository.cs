using Top2000.Api.GraphQL.GraphDb;
using Top2000.Api.GraphQL.Types;

namespace Top2000.Api.GraphQL.Repositories;

public class TrackRepository
{
    private readonly InMemoryDatabaseGraphDatabase database;

    public TrackRepository(InMemoryDatabaseGraphDatabase database)
    {
        this.database = database;
    }

    public Track? GetTrackById(int trackId)
    {
        var track = database.TrackNodes
            .FirstOrDefault(x => x.Id == trackId);

        if (track is null)
        {
            return null;
        }

        return Transform(track);
    }

    private Track Transform(Node node)
    {
        return new Track
        {
            Id = node.Id,
            Artist = (string)node.Properties["Artist"],
            Title = (string)node.Properties["Title"],
            RecordedYear = (int)node.Properties["RecordedYear"]
        };
    }

    public IEnumerable<Track> Search(TrackSearchType trackSearch)
    {
        return database.TrackNodes
            .Where(x =>
                ((string)x.Properties["Title"]).Contains(trackSearch.Title ?? "") ||
                ((string)x.Properties["Artist"]).Contains(trackSearch.Artist ?? ""))
            .Select(Transform);
    }
}
