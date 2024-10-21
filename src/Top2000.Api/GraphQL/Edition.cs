using Top2000.Api.GraphQL.Repositories;

namespace Top2000.Api.GraphQL;

public class Edition
{
    public int Year { get; set; }
    public DateTime StartUtcDateAndTime { get; set; }
    public DateTime EndUtcDateAndTime { get; set; }
    public bool HasPlayDateAndTime { get; set; }

    public IEnumerable<Listing> Listings([Service] ListingRepository listingRepository)
    {
        return listingRepository.GetAllListingByEditions(Year);
    }
}

public class Listing
{
    public int TrackId { get; set; }
    public int EditionYear { get; set; }
    public int Position { get; set; }
    public DateTime? PlayUtcDateAndTime { get; set; }
    public Track Track([Service] TrackRepository trackRepository)
    {
        return trackRepository.GetTrackById(TrackId)!;
    }
    public Edition Edition([Service] EditionRepository editionRepository)
    {
        return editionRepository.GetByYear(EditionYear)!;
    }
}

public class Track
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Artist { get; set; }
    public int RecordedYear { get; set; }

    public IEnumerable<Listing> Listings([Service] ListingRepository listingRepository)
    {
        return listingRepository.GetAllListingByTrack(Id);
    }
}
