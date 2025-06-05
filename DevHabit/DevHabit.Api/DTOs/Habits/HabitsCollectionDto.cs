namespace DevHabit.Api.DTOs.Habits;

public sealed record HabitsCollectionDto
{
    public required List<HabitDto> Data { get; init; }
}
