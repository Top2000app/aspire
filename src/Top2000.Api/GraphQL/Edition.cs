namespace Top2000.Api.GraphQL;

public class Edition
{
    public int Year { get; set; }
    public DateTime StartUtcDateAndTime { get; set; }
    public DateTime EndUtcDateAndTime { get; set; }
    public bool HasPlayDateAndTime { get; set; }

    public async Task<IEnumerable<TrackListing>> Tracks([Service] ListingRepository listingRepository)
    {
        return await listingRepository.GetAllListingByEditions(Year);
    }
}

public class TrackListing
{
    public int TrackId { get; set; }

    public int Position { get; set; }

    public int? Delta { get; set; }

    public DateTime PlayUtcDateAndTime { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Artist { get; set; } = string.Empty;
}
