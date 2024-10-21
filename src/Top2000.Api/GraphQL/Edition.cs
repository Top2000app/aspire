namespace Top2000.Api.GraphQL;

public class Edition
{
    public int Year { get; set; }
    public DateTime StartUtcDateAndTime { get; set; }
    public DateTime EndUtcDateAndTime { get; set; }
    public bool HasPlayDateAndTime { get; set; }

    public async Task<IEnumerable<TrackListing>> Tracks([Service] TrackListingRepository listingRepository)
    {
        return await listingRepository.GetAllListingByEditions(Year);
    }
}

public class EditionPosition : Edition
{
    public int Position { get; set; }
}

public class Listing
{
    public int TrackId { get; set; }
    public int Edition { get; set; }
    public int Position { get; set; }
    public DateTime? PlayUtcDateAndTime { get; set; }
}

public class Track
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Artist { get; set; }
    public int RecordedYear { get; set; }

    public async Task<IEnumerable<EditionPosition>> EditionsAsync([Service] TrackListingRepository listingRepository)
    {
        return await listingRepository.GetEditionsByTrackId(Id);
    }
}

public class TrackListing
{
    public int TrackId { get; set; }

    public int Position { get; set; }

    public DateTime PlayUtcDateAndTime { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Artist { get; set; } = string.Empty;
}
