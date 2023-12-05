using aoc23.helpers;

public class Day5
{
    public static long RunPart1()
    {
        var input = FileReader.ReadAllLines(5);

        var seeds = input[0].Split(':', StringSplitOptions.TrimEntries)[1].Split(' ', StringSplitOptions.TrimEntries).Select(long.Parse);

        var maps = new Dictionary<string, List<Mapping>>();

        _ = input.Skip(2).Aggregate("", (s, r) =>
        {
            if (string.IsNullOrEmpty(r))
            {
                return s;
            }
            if (char.IsLetter(r[0]))
            {
                maps.Add(r, []);
                return r;
            }

            var values = r.Split(' ').Select(long.Parse).ToArray();
            maps[s].Add(new Mapping(values[1], values[0], values[2]));

            return s;
        });

        var min = long.MaxValue;

        foreach (var item in seeds)
        {
            var pos = item;
            foreach (var map in maps)
            {
                pos = GetNext(pos, map.Value);
            }
            min = pos < min ? pos : min;
        }

        return min;
    }

    private static long GetNext(long current, List<Mapping> mappings)
    {
        var inMappingRange = mappings.SingleOrDefault(m => current >= m.Source && current < m.Source + m.Range);
        if (inMappingRange != null)
        {
            var offset = inMappingRange.Destination - inMappingRange.Source;
            return current + offset;
        }
        return current;
    }

    public static long RunPart2()
    {
        var input = FileReader.ReadAllLines(5);

        var seedsPair = input[0].Split(':', StringSplitOptions.TrimEntries)[1].Split(' ', StringSplitOptions.TrimEntries).Select(long.Parse).Chunk(2).Select(c => (start: c.First(), range: c.Last()));

        var maps = new Dictionary<string, List<Mapping>>();

        _ = input.Skip(2).Aggregate("", (s, r) =>
        {
            if (string.IsNullOrEmpty(r))
            {
                return s;
            }
            if (char.IsLetter(r[0]))
            {
                maps.Add(r, []);
                return r;
            }

            var values = r.Split(' ').Select(long.Parse).ToArray();
            maps[s].Add(new Mapping(values[1], values[0], values[2]));

            return s;
        });

        var min = long.MaxValue;

        List<Slice> slices = seedsPair.Select(sp => new Slice(sp.start, sp.range)).ToList();
        foreach (var item in maps)
        {
            slices = GetNextSlice(slices, item.Value);
        }

        var sliceMin = slices.MinBy(s => s.StartIndex).StartIndex;
        min = sliceMin < min ? sliceMin : min;

        return min;
    }

    private static List<Slice> GetNextSlice(List<Slice> currentSlices, List<Mapping> mappings)
    {
        var orderedMapping = mappings.OrderBy(m => m.Source);
        var nextSlices = new List<Slice>();
        foreach (var slice in currentSlices)
        {
            nextSlices.AddRange(FindCovered(slice, orderedMapping));
        }
        return nextSlices;
    }

    static IEnumerable<Slice> FindCovered(Slice slice, IOrderedEnumerable<Mapping> mappings)
    {
        foreach (var map in mappings)
        {
            var offset = map.Destination - map.Source;

            if (slice.StartIndex >= map.Source && slice.StartIndex < map.Source + map.Range)
            {
                if (slice.StartIndex + slice.Range > map.Source + map.Range)
                {
                    var cutRange = map.Source + map.Range - slice.StartIndex;
                    yield return new Slice(slice.StartIndex + offset, cutRange);
                }
                else
                {
                    yield return new Slice(slice.StartIndex + offset, slice.Range);
                }
            }

            if (slice.StartIndex < map.Source && slice.StartIndex + slice.Range > map.Source)
            {
                if (slice.StartIndex + slice.Range > map.Source + map.Range)
                {
                    yield return new Slice(map.Destination, map.Range);
                }
                else
                {
                    var cutRange = slice.StartIndex + slice.Range - map.Source;
                    yield return new Slice(map.Destination, cutRange);
                }
            }
        }

        var last = mappings.Last();
        if (slice.StartIndex > last.Source + last.Range)
        {
            yield return new Slice(slice.StartIndex, slice.Range);
        }
        var first = mappings.First();
        if (slice.StartIndex < first.Source)
        {
            yield return new Slice(slice.StartIndex, slice.Range);
        }
    }

    record Slice(long StartIndex, long Range);
    record Mapping(long Source, long Destination, long Range);
}
