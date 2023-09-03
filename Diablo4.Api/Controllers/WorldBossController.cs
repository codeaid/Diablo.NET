using Diablo4.Shared.Collections;
using Diablo4.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Diablo4.Api.Controllers;

[ApiController]
[Route("/world-boss")]
public class WorldBossController
{
    /// <summary>
    /// Lists world bosses spawning in the next 48 hours (as well as the currently active one if available).
    /// </summary>
    /// <param name="worldBossIntervalSequencer">World boss interval sequencer.</param>
    /// <param name="worldBossSequencer">World boss sequencer.</param>
    /// <returns></returns>
    [HttpGet]
    public IResult List(WorldBossIntervalSequencer worldBossIntervalSequencer, WorldBossSequencer worldBossSequencer)
    {
        // Calculate start and end spawn time (accounting for a currently active boss).
        DateTime minSpawnTime = DateTime.UtcNow.Subtract(new TimeSpan(0, 15, 0));
        DateTime maxSpawnTime = DateTime.UtcNow.Add(new TimeSpan(48, 0, 0));

        // Select world boss spawns matching the specified time criteria.
        IEnumerable<WorldBossSpawn> spawns =
            new WorldBossSpawnEnumerable(worldBossIntervalSequencer, worldBossSequencer, maxSpawnTime)
                .Where(spawn => spawn.Time > minSpawnTime)
                .Select(spawn => new WorldBossSpawn(spawn.Boss, spawn.Time.ToLocalTime()));

        return Results.Json(spawns);
    }
}
