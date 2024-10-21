using Top2000.Api.GraphQL.Types;

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

    public async Task<IEnumerable<Track>> SearchForTrack(TrackSearchType searchinput, [Service] TrackRepository trackRepository)
    {
        return await trackRepository.SearchAsync(searchinput);
    }
}

