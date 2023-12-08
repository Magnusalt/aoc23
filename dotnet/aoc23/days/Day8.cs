using System.Numerics;
using aoc23.helpers;

public static class Day8
{
    public static int RunPart1()
    {
        var input = FileReader.ReadAllLines(8);

        var direction = input[0];

        var map = input.Skip(2).Select(i => i.Split('=', StringSplitOptions.TrimEntries)).ToDictionary(s => s[0], s => Parse(s[1]));

        var current = map["AAA"];
        var next = direction[0] == 'L' ? current.left : current.right;
        var index = 0;
        var steps = 1;
        while (next != "ZZZ")
        {
            current = map[next];
            index = index < direction.Length - 1 ? index + 1 : 0;
            next = direction[index] == 'L' ? current.left : current.right;
            steps++;
        }

        return steps;
    }

    private static (string left, string right) Parse(string v)
    {
        var split = v.Split(',', StringSplitOptions.TrimEntries);

        var res = (split[0][1..], split[1][..^1]);
        return res;
    }

    public static long RunPart2()
    {
        var input = FileReader.ReadAllLines(8);

        var direction = input[0];

        var map = input.Skip(2).Select(i => i.Split('=', StringSplitOptions.TrimEntries)).ToDictionary(s => s[0], s => Parse(s[1]));

        var startKeys = map.Keys.Where(k => k.EndsWith('A'));

        var res = new List<int>();
        foreach (var item in startKeys)
        {
            res.Add(GetSteps(item, map, direction));
        }

        var max = res.Max();
        long count = max;

        while (!res.All(i => count % i == 0) && count < long.MaxValue)
        {
            count += max;
        }

        return count;
    }

    private static int GetSteps(string startKey, Dictionary<string, (string left, string right)> map, string directions)
    {

        var current = map[startKey];
        var next = directions[0] == 'L' ? current.left : current.right;
        var index = 0;
        var steps = 1;
        while (!next.EndsWith('Z'))
        {
            current = map[next];
            index = index < directions.Length - 1 ? index + 1 : 0;
            next = directions[index] == 'L' ? current.left : current.right;
            steps++;
        }
        return steps;
    }
}