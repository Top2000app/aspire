using Top2000.Api.GraphQL.Repositories;
using Top2000.Api.GraphQL.Types;

namespace Top2000.Api.GraphQL;

public class Query
{
    public Edition Latest([Service] EditionRepository editionRepository)
    {
        return editionRepository.Latest();
    }

    public IEnumerable<Edition> Editions([Service] EditionRepository editionRepository)
    {
        return editionRepository.AllEditions();
    }

    public Edition? Current([Service] EditionRepository editionRepository)
    {
        return editionRepository.Current();
    }

    public Edition? GetEditionByYear([Service] EditionRepository editionRepository, int year)
    {
        return editionRepository.GetByYear(year);
    }

    public IEnumerable<Track> SearchForTrack(TrackSearchType searchinput, [Service] TrackRepository trackRepository)
    {
        return trackRepository.Search(searchinput);
    }
}

