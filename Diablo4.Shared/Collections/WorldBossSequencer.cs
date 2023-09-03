using System.Collections.Immutable;
using Diablo4.Shared.Types;

namespace Diablo4.Shared.Collections;

public class WorldBossSequencer
{
    /// <summary>
    /// Number of repetitions for each individual world boss in the <see cref="SpawnSequence"/> list.
    /// </summary>
    private static readonly ImmutableArray<int> SpawnCounts = ImmutableArray.Create(3, 2, 3, 2, 3, 2);

    /// <summary>
    /// World boss spawn order.
    /// </summary>
    private static readonly ImmutableList<WorldBoss> SpawnOrder =
        ImmutableList.Create(
            WorldBoss.Ashava,
            WorldBoss.WanderingDeath,
            WorldBoss.Avarice);

    /// <summary>
    /// Sequence of world bosses to spawn.
    /// </summary>
    private static readonly ImmutableList<WorldBoss> SpawnSequence =
        SpawnCounts
            .SelectMany((times, index) => Enumerable.Repeat(SpawnOrder[index % SpawnOrder.Count], times))
            .ToImmutableList();

    /// <summary>
    /// Indexer to retrieve the spawn sequence at the specified position.
    /// </summary>
    /// <param name="index">Target spawn sequence index.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if index is a negative number.</exception>
    public WorldBoss this[int index]
    {
        get
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException($"Invalid world boss spawn index specified: {index}");
            }

            return SpawnSequence[index % SpawnSequence.Count];
        }
    }
}
