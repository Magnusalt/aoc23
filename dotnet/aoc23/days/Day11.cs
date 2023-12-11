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


        return combinations.Select(p => Math.Abs(p.First().x - p.Last().x) + Math.Abs(p.First().y - p.Last().y)).Sum();
    }

    public static long RunPart2()
    {
        var input = FileReader.ReadAllLines(11);

        var rowIndices = new List<int>();
        var rowIndex = 0;
        foreach (var row in input)
        {
            if (row.All(c => c == '.'))
            {
                rowIndices.Add(rowIndex);
            }
            rowIndex++;
        }
        var rowLength = input[0].Length;
        var colsWithout = new List<int>();
        for (int i = 0; i < rowLength; i++)
        {
            var allEmpty = input.Select(r => r[i]).All(r => r == '.');
            if (allEmpty)
            {
                colsWithout.Add(i);
            }
        }

        var galaxies = new List<(int x, int y)>();

        for (int y = 0; y < input.Length; y++)
        {
            string row = input[y];
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

        long sum = 0;
        foreach (var combination in combinations)
        {
            var gal1 = combination.First();
            var gal2 = combination.Last();

            var x1 = Math.Min(gal1.x, gal2.x);
            var x2 = Math.Max(gal1.x, gal2.x);
            var y1 = Math.Min(gal1.y, gal2.y);
            var y2 = Math.Max(gal1.y, gal2.y);

            long millionsCrossingX = colsWithout.Where(col => col > x1 && col < x2).Count() * 999_999;
            long millionsCrossingY = rowIndices.Where(row => row > y1 && row < y2).Count() * 999_999;

            var dY = y2 - y1;
            var dX = x2 - x1;

            sum += dX + dY + millionsCrossingX + millionsCrossingY;

        }

        return sum;
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