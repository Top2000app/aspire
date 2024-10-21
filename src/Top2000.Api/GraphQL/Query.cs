namespace Top2000.Api.GraphQL;

public class Query
{
    public async Task<Edition> LatestAsync([Service] EditionRepository editionRepository)
    {
        return await editionRepository.LatestAsync();
    }

    public async Task<IEnumerable<Edition>> EditionsAsync([Service] EditionRepository editionRepository)
    {
        return await editionRepository.AllEditionsAsync();
    }

    public async Task<Edition?> CurrentAsync([Service] EditionRepository editionRepository)
    {
        return await editionRepository.CurrentAsync();
    }

    public async Task<Edition?> GetEditionByYear([Service] EditionRepository editionRepository, int year)
    {
        return await editionRepository.GetByYear(year);
    }
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
}