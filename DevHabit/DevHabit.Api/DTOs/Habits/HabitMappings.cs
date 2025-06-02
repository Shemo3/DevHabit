using System.Linq.Expressions;
using DevHabit.Api.Entities;

namespace DevHabit.Api.DTOs.Habits;

internal static class HabitMappings
{
    public static Habit ToEntity(this CreateHabitDto createHabitDto)
    {
        return new Habit()
        {
            Id = $"h_{Guid.CreateVersion7()}",
            Name = createHabitDto.Name,
            Description = createHabitDto.Description,
            Type = createHabitDto.Type,
            Frequency = new Frequency
            {
                Type = createHabitDto.Frequency.Type,
                TimesPerPeriod = createHabitDto.Frequency.TimesPerPeriod
            },
            Target = new Target
            {
                Value = createHabitDto.Target.Value,
                Unit = createHabitDto.Target.Unit
            },
            Status = HabitStatus.OnGoing,
            IsArchived = false,
            EndDate = createHabitDto.EndDate,
            Milestone = createHabitDto.Milestone is not null
                ? new Milestone
                {
                    Target = createHabitDto.Milestone.Target,
                    Current = 0 // Initialize current progress to 0
                }
                : null,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    public static HabitDto ToDto(this Habit habit)
    {
        return new HabitDto()
        {
            Id = habit.Id,
            Name = habit.Name,
            Description = habit.Description,
            Type = habit.Type,
            Frequency = new FrequencyDto
            {
                Type = habit.Frequency.Type,
                TimesPerPeriod = habit.Frequency.TimesPerPeriod
            },
            Target = new TargetDto
            {
                Value = habit.Target.Value,
                Unit = habit.Target.Unit
            },
            Status = habit.Status,
            IsArchived = habit.IsArchived,
            EndDate = habit.EndDate,
            Milestone = habit.Milestone == null
                ? null
                : new MilestoneDto
                {
                    Target = habit.Milestone.Target,
                    Current = habit.Milestone.Current
                },
            CreatedAtUtc = habit.CreatedAtUtc,
            UpdatedAtUtc = habit.UpdatedAtUtc,
            LastCompletedAtUtc = habit.LastCompletedAtUtc
        };
    }

}
