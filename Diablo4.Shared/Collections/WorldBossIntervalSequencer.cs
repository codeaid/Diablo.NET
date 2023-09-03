using System.Collections.Immutable;

namespace Diablo4.Shared.Collections;

public class WorldBossIntervalSequencer
{
    /// <summary>
    /// The longer spawn interval.
    /// </summary>
    private static readonly TimeSpan IntervalA = new TimeSpan(0, 5, 53, 29, 500);

    /// <summary>
    /// The shorter spawn interval.
    /// </summary>
    private static readonly TimeSpan IntervalB = new TimeSpan(0, 5, 25, 13, 400);

    /// <summary>
    /// List of time intervals used when calculating world boss spawn times.
    /// </summary>
    private static readonly ImmutableList<TimeSpan> IntervalSequence =
        ImmutableList.Create(
            IntervalA,
            IntervalA,
            IntervalB,
            IntervalA,
            IntervalB);

    /// <summary>
    /// Indexer to retrieve the spawn interval at the specified position.
    /// </summary>
    /// <param name="index">Target spawn interval index.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if index is a negative number.</exception>
    public TimeSpan this[int index]
    {
        get
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException($"Invalid world boss interval index specified: {index}");
            }

            return IntervalSequence[index % IntervalSequence.Count];
        }
    }
}
