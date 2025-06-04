using DevHabit.Api.Database;
using DevHabit.Api.DTOs.HabitTags;
using DevHabit.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevHabit.Api.Controllers;

[ApiController]
[Route("habits/{habitId}/tags")]
public sealed class HabitTagsController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public HabitTagsController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    [HttpPut]
    public async Task<ActionResult> UpsertHabitTags(string habitId, UpsertHabitTagsDto upsertHabitTagsDto)
    {
        Habit? habit = await _dbContext.Habits
            .FirstOrDefaultAsync(h => h.Id == habitId);
        if (habit is null)
        {
            return NotFound();
        }

        HashSet<string> currentTagIds = await _dbContext.HabitTags.Select(ht => ht.TagId)
            .ToHashSetAsync();

        if (currentTagIds.SetEquals(upsertHabitTagsDto.TagIds))
        {
            return NoContent();
        }

        List<string> existingIds = await _dbContext
            .Tags
            .Where(t => upsertHabitTagsDto.TagIds.Contains(t.Id))
            .Select(t => t.Id)
            .ToListAsync();

        if (existingIds.Count != upsertHabitTagsDto.TagIds.Count)
        {
            return BadRequest("One or more Tag Ids are invalid");
        }

        habit.HabitTags.RemoveAll(ht => !upsertHabitTagsDto.TagIds.Contains(ht.TagId));
        string[] tagIdsToAdd = upsertHabitTagsDto.TagIds.Except(currentTagIds).ToArray();
        habit.HabitTags.AddRange(tagIdsToAdd.Select(t => new HabitTag()
        {
            HabitId = habit.Id,
            TagId = t,
            CreatedAtUtc = DateTime.UtcNow
        }));

        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{tagId}")]
    public async Task<ActionResult> RemoveTagFromHabit(string habitId, string tagId)
    {
        HabitTag? habitTag = await _dbContext.HabitTags
            .SingleOrDefaultAsync(h => h.HabitId == habitId && h.TagId == tagId);
        if (habitTag is null)
        {
            return NotFound();
        }

        _dbContext.HabitTags.Remove(habitTag);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }
}
