using System.Xml.Schema;
using aoc23.helpers;

public static class Day9
{
    public static int RunPart1()
    {
        var input = FileReader.ReadAllLines(9);

        var total = 0;
        foreach (var item in input)
        {
            total += GetNextInSequence(item);
        }

        return total;
    }

    public static int RunPart2()
    {
        var input = FileReader.ReadAllLines(9);

        var total = 0;
        foreach (var item in input)
        {
            total += GetPrevInSequence(item);
        }

        return total;
    }

    private static int GetNextInSequence(string item)
    {
        var parsed = item.Split(' ').Select(int.Parse).ToList();
        var diffrences = new List<List<int>>
        {
            parsed
        };

        var currentList = parsed;

        while (!currentList.All(i => i == 0))
        {
            var next = new List<int>();
            for (int i = 1; i < currentList.Count; i++)
            {
                var diff = currentList[i] - currentList[i - 1];
                next.Add(diff);
            }
            diffrences.Add(next);
            currentList = next;
        }

        diffrences.Reverse();

        var res = diffrences.Skip(1).Aggregate(0, (s, i) => s += i.Last());
        return res;
    }
    private static int GetPrevInSequence(string item)
    {
        var parsed = item.Split(' ').Select(int.Parse).ToList();
        var diffrences = new List<List<int>>
        {
            parsed
        };

        var currentList = parsed;

        while (!currentList.All(i => i == 0))
        {
            var next = new List<int>();
            for (int i = 1; i < currentList.Count; i++)
            {
                var diff = currentList[i] - currentList[i - 1];
                next.Add(diff);
            }
            diffrences.Add(next);
            currentList = next;
        }

        diffrences.Reverse();

        for (int i = 1; i < diffrences.Count; i++)
        {
            var currentLevel = diffrences[i];
            var below = diffrences[i - 1];
            var newFirst = currentLevel[0] - below[0];
            currentLevel.Insert(0, newFirst);
        }

        return diffrences.Last()[0];
    }
}