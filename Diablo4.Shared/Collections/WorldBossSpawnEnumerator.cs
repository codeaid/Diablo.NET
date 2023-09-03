using System.Collections;
using Diablo4.Shared.Models;
using Diablo4.Shared.Services;

namespace Diablo4.Shared.Collections;

public class WorldBossSpawnEnumerator : IEnumerator<WorldBossSpawn>
{
    /// <inheritdoc />
    public WorldBossSpawn Current { get; private set; } = null!;

    /// <inheritdoc />
    object IEnumerator.Current => Current;

    /// <summary>
    /// Number of iterations performed on the current enumerator.
    /// </summary>
    private int CurrentPosition { get; set; }

    /// <summary>
    /// Maximum time to generate world boss spawns up to.
    /// </summary>
    private DateTime? MaxSpawnTime { get; }

    /// <summary>
    /// World boss interval sequencer.
    /// </summary>
    private WorldBossIntervalSequencer WorldBossIntervalSequencer { get; set; }

    /// <summary>
    /// World boss sequencer.
    /// </summary>
    private WorldBossSequencer WorldBossSequencer { get; set; }

    /// <summary>
    /// Class constructor.
    /// </summary>
    /// <param name="worldBossIntervalSequencer">World boss interval sequencer.</param>
    /// <param name="worldBossSequencer">World boss sequencer.</param>
    /// <param name="maxSpawnTime">Maximum time to generate world boss spawns up to.</param>
    public WorldBossSpawnEnumerator(
        WorldBossIntervalSequencer worldBossIntervalSequencer,
        WorldBossSequencer worldBossSequencer,
        DateTime? maxSpawnTime = null)
    {
        WorldBossIntervalSequencer = worldBossIntervalSequencer;
        WorldBossSequencer = worldBossSequencer;

        // Generate spawns up to a year ahead if no custom limits are specified.
        MaxSpawnTime = maxSpawnTime ?? DateTime.UtcNow.AddYears(1);
        Reset();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public bool MoveNext()
    {
        // Retrieve the next spawn interval to add to the current spawn time.
        TimeSpan nextSpawnInterval = WorldBossIntervalSequencer[CurrentPosition];

        // Calculate the next provisional spawn time.
        DateTime nextSpawnTime = Current.Time.Add(nextSpawnInterval);

        // Ensure next spawn time occurs within one of the allowed windows.
        if (!WorldBossSpawnService.IsValidSpawnTime(nextSpawnTime))
        {
            nextSpawnTime = nextSpawnTime.Add(WorldBossSpawnService.SpawnWindowOffset);
        }

        // Terminate enumeration if the next spawn time falls outside of the requested window.
        if (nextSpawnTime > MaxSpawnTime)
        {
            return false;
        }

        // Increment number of iterations performed on the current enumerator.
        CurrentPosition++;

        // Generate a new world boss info object and store it as the current value.
        Current = new WorldBossSpawn(WorldBossSequencer[CurrentPosition], nextSpawnTime);

        return true;
    }

    /// <inheritdoc />
    public void Reset()
    {
        // Reset the enumerator to the very first boss spawn.
        CurrentPosition = 0;
        Current = new WorldBossSpawn(WorldBossSequencer[CurrentPosition], WorldBossSpawnService.BigBang);
    }
}
