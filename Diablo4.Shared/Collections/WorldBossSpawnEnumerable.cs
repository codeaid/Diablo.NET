using System.Collections;
using Diablo4.Shared.Models;

namespace Diablo4.Shared.Collections;

public class WorldBossSpawnEnumerable : IEnumerable<WorldBossSpawn>
{
    /// <summary>
    /// World boss spawn enumerator.
    /// </summary>
    private WorldBossSpawnEnumerator Enumerator { get; }

    /// <summary>
    /// Class constructor.
    /// </summary>
    /// <param name="worldBossIntervalSequencer">World boss interval sequencer.</param>
    /// <param name="worldBossSequencer">World boss sequencer.</param>
    /// <param name="maxSpawnTime">Upper range of the world boss spawn time to include in the output.</param>
    public WorldBossSpawnEnumerable(
        WorldBossIntervalSequencer worldBossIntervalSequencer,
        WorldBossSequencer worldBossSequencer,
        DateTime? maxSpawnTime = null)
    {
        Enumerator = new WorldBossSpawnEnumerator(worldBossIntervalSequencer, worldBossSequencer, maxSpawnTime);
    }

    /// <inheritdoc />
    public IEnumerator<WorldBossSpawn> GetEnumerator() => Enumerator;

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
