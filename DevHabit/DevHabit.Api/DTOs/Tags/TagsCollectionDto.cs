namespace DevHabit.Api.DTOs.Tags;

public sealed record TagsCollectionDto
{
    public List<TagDto> Data { get; init; }
}
