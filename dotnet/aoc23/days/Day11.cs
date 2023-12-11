using aoc23.helpers;
public static class Day11
{
    public static int RunPart1()
    {
        var input = FileReader.ReadAllLines(11);

        var expanded = new List<string>();
        var rowLength = input[0].Length;
        var expansionLine = new string('.', rowLength);
        foreach (var item in input)
        {
            if (item.All(c => c == '.'))
            {
                expanded.Add(item);
                expanded.Add(expansionLine);
            }
            else
            {
                expanded.Add(item);
            }
        }

        var colsWithout = new List<int>();
        for (int i = 0; i < rowLength; i++)
        {
            var allEmpty = input.Select(r => r[i]).All(r => r == '.');
            if (allEmpty)
            {
                colsWithout.Add(i);
            }
        }

        var indexCount = 0;
        foreach (var colIndex in colsWithout)
        {
            expanded = expanded.Select(r => r.Insert(colIndex + indexCount, ".")).ToList();
            indexCount++;
        }

        var galaxies = new List<(int x, int y)>();

        for (int y = 0; y < expanded.Count; y++)
        {
            string row = expanded[y];
            for (int x = 0; x < row.Length; x++)
            {
                char col = row[x];
                if (col == '#')
                {
                    galaxies.Add((x, y));
                }
            }
        }

        var combinations = GetCombinations(galaxies, 2);


        return combinations.Select(p=> Math.Abs(p.First().x - p.Last().x) + Math.Abs(p.First().y - p.Last().y)).Sum();
    }

    public static int RunPart2()
    {
        return 0;
    }
    
    static IEnumerable<IEnumerable<T>> GetCombinations<T>(IEnumerable<T> items, int count)
    {
        int i = 0;
        foreach (var item in items)
        {
            if (count == 1)
                yield return new T[] { item };
            else
            {
                foreach (var result in GetCombinations(items.Skip(i + 1), count - 1))
                    yield return new T[] { item }.Concat(result);
            }

            ++i;
        }
    }
}