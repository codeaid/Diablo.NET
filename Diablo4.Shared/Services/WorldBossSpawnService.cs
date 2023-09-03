using System.Collections.Immutable;

namespace Diablo4.Shared.Services;

public static class WorldBossSpawnService
{
    /// <summary>
    /// Starting time of all the world boss spawn calculations.
    /// </summary>
    public static readonly DateTime BigBang = new(2023, 6, 2, 4, 36, 49, 500, DateTimeKind.Utc);

    /// <summary>
    /// Time offset to apply to the calculated world boss spawn time if it falls outside of permitted window.
    /// </summary>
    public static readonly TimeSpan SpawnWindowOffset = new(0, 2, 0, 0);

    /// <summary>
    /// List of permitted world boss spawn windows (UTC time).
    /// </summary>
    private static readonly ImmutableList<(TimeOnly, TimeOnly)> WindowPool =
        ImmutableList.Create(
            (new TimeOnly(4, 30), new TimeOnly(6, 30)),
            (new TimeOnly(10, 30), new TimeOnly(12, 30)),
            (new TimeOnly(16, 30), new TimeOnly(18, 30)),
            (new TimeOnly(22, 30), new TimeOnly(0, 30)));

    /// <summary>
    /// Determine if the specified spawn time is within any of the allowed spawn windows.
    /// </summary>
    /// <param name="spawn">Spawn time to validate.</param>
    /// <returns>True if the spawn time is valid, False otherwise.</returns>
    public static bool IsValidSpawnTime(DateTime spawn) =>
        WindowPool.Any(window => IsInSpawnWindow(spawn, window));

    /// <summary>
    /// Check if a spawn time is within the specified spawn window.
    /// </summary>
    /// <param name="spawn">Spawn time to validate.</param>
    /// <param name="window">Target spawn window to validate spawn time against.</param>
    /// <returns>True if the spawn time is within the window, False otherwise.</returns>
    private static bool IsInSpawnWindow(DateTime spawn, (TimeOnly, TimeOnly) window)
    {
        (TimeOnly windowStart, TimeOnly windowEnd) = window;
        TimeOnly spawnTime = TimeOnly.FromDateTime(spawn);

        return spawnTime.IsBetween(windowStart, windowEnd);
    }
}
