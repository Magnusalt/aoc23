using System.Numerics;
using aoc23.helpers;

namespace aoc23.days;

static class Day6
{
    public static int RunPart1()
    {
        var input = FileReader.ReadAllLines(6);

        var maxT = input[0].Split(':', StringSplitOptions.TrimEntries)[1].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
        var distancesToBeat = input[1].Split(':', StringSplitOptions.TrimEntries)[1].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);

        var combo = maxT.Zip(distancesToBeat, (t, d) => (time: t, distance: d));

        var result = 1;

        foreach (var (time, distance) in combo)
        {
            result *= CountWaysToBeat(time, distance);
        }

        return result;
    }

    private static int CountWaysToBeat(int maxT, long distanceToBeat)
    {
        var count = 0;
        for (long i = 0; i < maxT; i++)
        {
            long distanceTraveled = maxT * i - (i * i);
            if (distanceTraveled > distanceToBeat)
            {
                count++;
            }
        }
        return count;
    }

    public static int RunPart2()
    {
        var input = FileReader.ReadAllLines(6);

        var maxT = input[0].Split(':', StringSplitOptions.TrimEntries)[1].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Aggregate("", (a, s) => a + s);
        var distancesToBeat = input[1].Split(':', StringSplitOptions.TrimEntries)[1].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Aggregate("", (a, s) => a + s);

        return CountWaysToBeat(int.Parse(maxT), long.Parse(distancesToBeat));
    }
}