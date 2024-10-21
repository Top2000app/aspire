namespace Top2000.Api.GraphQL.Types;

public class TrackSearchType
{
    public string? Artist { get; set; }
    public string? Title { get; set; }
}

public class NewEditionType
{
    public string? Year { get; set; }
    public bool? HasPlayDateAndTime { get; set; }
}