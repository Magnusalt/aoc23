using System.Security.Cryptography;
using System.Text;
using aoc23.helpers;

public static class Day12
{
    public static int RunPart1()
    {
        var input = FileReader.ReadAllLines(12);

        var total = 0;

        foreach (var row in input)
        {
            var split = row.Split(' ');

            var arrangement = string.Join('.', split[0].Split('.', StringSplitOptions.RemoveEmptyEntries));
            var grouping = split[1].Split(',').Select(int.Parse).ToList();

            var span = arrangement.Length - (grouping.Sum() + grouping.Count - 1);

            if (span == 0)
            {
                total += 1;
                continue;
            }

            var blocks = new List<Block>();

            var lastBlock = grouping.Skip(1).Aggregate(new Block(grouping[0], span) { Index = 0 }, (s, g) =>
            {
                blocks.Add(s);

                return new Block(g, s.Index + s.Length + 1 + span) { Index = s.Index + s.Length + 1 };
            });

            blocks.Add(lastBlock);

            var combos = CountCombinations(0, blocks, arrangement, span);
            total += combos;
        }


        return total;
    }

    private static int CountCombinations(int sum, List<Block> blocks, string fixture, int span)
    {
        var stringBuilder = new StringBuilder();
        for (int i = 0; i < fixture.Length; i++)
        {
            char item = fixture[i];
            var block = blocks.FirstOrDefault(b => b.Index == i);
            if (block != null)
            {
                var sub = fixture.Substring(block.Index, block.Length);
                if (sub.Contains('.'))
                {
                    goto Notpossible;
                }
                var toInsert = sub.Replace('?', '#');
                stringBuilder.Append(toInsert);
                i += block.Length - 1;
            }
            else
            {
                var itemToAppend = item switch
                {
                    '?' => '.',
                    _ => item
                };
                stringBuilder.Append(itemToAppend);
            }
        }
        var blocksInserted = stringBuilder.ToString();
        var reduced = blocksInserted.Split('.', StringSplitOptions.RemoveEmptyEntries);
        if (reduced.Length == blocks.Count && reduced.Select(b => b.Length).Zip(blocks, (i, b) => i == b.Length).All(e => e))
        {
            sum += 1;
        }

    Notpossible:

        if (blocks.All(b => b.Index == b.MaxIndex))
        {
            return sum;
        }

        var atEnd = blocks.FirstOrDefault(b => b.Index == b.MaxIndex);

        if (atEnd != null)
        {
            var indexOfAtEnd = blocks.IndexOf(atEnd);
            if (indexOfAtEnd > 0)
            {
                var nextBlocks = blocks.Take(indexOfAtEnd - 1).ToList();
                var nextBlockToIncrease = blocks[indexOfAtEnd - 1];
                nextBlockToIncrease.Index += 1;

                var lastBlock = blocks.Skip(indexOfAtEnd).Aggregate(nextBlockToIncrease, (s, g) =>
                {
                    nextBlocks.Add(s);

                    return new Block(g.Length, g.MaxIndex) { Index = s.Index + s.Length + 1 };
                });

                nextBlocks.Add(lastBlock);
                blocks = nextBlocks;
            }
            else
            {
                var nextBlocks = new List<Block>();
                var nextBlockToIncrease = blocks[indexOfAtEnd];
                nextBlockToIncrease.Index += 1;

                var lastBlock = blocks.Skip(indexOfAtEnd).Aggregate(nextBlockToIncrease, (s, g) =>
                {
                    nextBlocks.Add(s);

                    return new Block(g.Length, g.MaxIndex) { Index = s.Index + s.Length + 1 };
                });

                nextBlocks.Add(lastBlock);
                blocks = nextBlocks;
            }
        }
        else
        {
            blocks.Last().Index += 1;
        }

        return CountCombinations(sum, blocks, fixture, span);
    }

    private record Block(int Length, int MaxIndex)
    {
        public int Index { get; set; }
    }

    public static long RunPart2()
    {
        var input = FileReader.ReadAllLines(12);

        long total = 0;
        var i = 0;
        foreach (var row in input)
        {
            var split = row.Split(' ');

            var timesFiveArr = string.Join('?', Enumerable.Range(0, 5).Select(_ => split[0]));
            var timesFiveGroup = string.Join(',', Enumerable.Range(0, 5).Select(_ => split[1]));

            var arrangement = string.Join('.', timesFiveArr.Split('.', StringSplitOptions.RemoveEmptyEntries));
            var grouping = timesFiveGroup.Split(',').Select(int.Parse).ToList();

            var cache = new Dictionary<string, long>();
            total += CountCombosWithCache(arrangement, grouping, cache);
            i++;
        }


        return total;
    }

    static long CountCombosWithCache(string springs, List<int> groups, Dictionary<string, long> cache)
    {
        var key = $"{springs},{string.Join(',', groups)}";
        if (cache.TryGetValue(key, out var value))
        {
            return value;
        }

        value = GetCount(springs, groups, cache);
        cache[key] = value;

        return value;
    }

    static long GetCount(string springs, List<int> groups, Dictionary<string, long> cache)
    {

        while (true)
        {
            if (groups.Count == 0)
            {
                return springs.Contains('#') ? 0 : 1;
            }

            if (string.IsNullOrEmpty(springs))
            {
                return 0;
            }

            if (springs.StartsWith('.'))
            {
                springs = springs.Trim('.');
                continue;
            }

            if (springs.StartsWith('?'))
            {
                return CountCombosWithCache("." + springs[1..], groups, cache) + CountCombosWithCache("#" + springs[1..], groups, cache);
            }

            if (springs.StartsWith('#'))
            {
                if (groups.Count == 0)
                {
                    return 0;
                }

                if (springs.Length < groups[0])
                {
                    return 0;
                }

                if (springs[..groups[0]].Contains('.'))
                {
                    return 0;
                }

                if (groups.Count > 1)
                {
                    if (springs.Length < groups[0] + 1 || springs[groups[0]] == '#')
                    {
                        return 0;
                    }

                    springs = springs[(groups[0] + 1)..];
                    groups = groups[1..];
                    continue;
                }

                springs = springs[groups[0]..];
                groups = groups[1..];
                continue;
            }

            throw new Exception("Invalid input");
        }
    }
}