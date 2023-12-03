using aoc23.helpers;

static class Day3
{
    public static int RunPart1()
    {
        var input = FileReader.ReadAllLines(3);

        var maxY = input.Length - 1;
        var maxX = input[0].Length - 1;
        var y = 0;
        var sum = 0;
        foreach (var line in input)
        {
            for (int x = 0; x < line.Length; x++)
            {
                char c = line[x];
                if (char.IsNumber(c))
                {
                    var number = new List<char> { c };
                    x += 1;
                    while (x <= maxX && char.IsDigit(line[x]))
                    {
                        number.Add(line[x]);
                        x += 1;
                    }
                    var surrounding = new List<char>();
                    for (int j = y - 1; j <= y + 1; j++)
                    {
                        for (int k = x - number.Count - 1; k <= x; k++)
                        {
                            if (j >= 0 && j <= maxY && k >= 0 && k <= maxX)
                            {
                                if (j == y && k > x - number.Count - 1 && k < x)
                                {
                                    continue;
                                }
                                surrounding.Add(input[j][k]);
                            }
                        }
                    }
                    if (surrounding.Any(c => c != '.'))
                    {
                        sum += int.Parse(new string(number.ToArray()));
                    }
                }
            }
            y++;
        }
        return sum;
    }

    /// <summary>
    /// No cog wheels on top or bottom row, no cog wheels without at least 3 spaces to left/right, no numbers above 999
    /// </summary>
    /// <returns></returns>
    public static int RunPart2()
    {
        var input = FileReader.ReadAllLines(3);
        var maxY = input.Length - 1;
        var maxX = input[0].Length - 1;
        var y = 0;
        var sum = 0;

        foreach (var line in input)
        {
            for (int x = 0; x < maxX; x++)
            {
                var current = line[x];

                if (current == '*')
                {
                    var range = (x - 1)..(x + 2);
                    var above = ParseRow(input[y - 1], range);
                    var same = ParseRow(input[y], range);
                    var below = ParseRow(input[y + 1], range);
                    var allAround = above.Concat(same).Concat(below).ToList();
                    if (allAround.Count == 2)
                    {
                        sum += allAround.Aggregate(1, (a, f) => a * f);
                    }
                }
            }
            y++;
        }

        return sum;
    }

    private static int[] ParseRow(string row, Range range)
    {
        var slice = row[range];

        int[] res = slice switch
        {
        [var a, var b, var c] when !char.IsNumber(a) && !char.IsNumber(b) && !char.IsNumber(c) => [],
        [var a, var b, var c] when char.IsNumber(a) && char.IsNumber(b) && char.IsNumber(c) => [int.Parse(new string([a, b, c]))],
        [var a, var b, var c] when char.IsNumber(a) && !char.IsNumber(b) && char.IsNumber(c) => [GetNumberLeftSide(range.Start.Value, row), GetNumberRightSide(range.End.Value - 1, row)],
        [var a, var b, var c] when char.IsNumber(a) && char.IsNumber(b) && !char.IsNumber(c) => [GetNumberLeftSide(range.Start.Value + 1, row)],
        [var a, var b, var c] when char.IsNumber(a) && !char.IsNumber(b) && !char.IsNumber(c) => [GetNumberLeftSide(range.Start.Value, row)],
        [var a, var b, var c] when !char.IsNumber(a) && char.IsNumber(b) && char.IsNumber(c) => [GetNumberRightSide(range.End.Value - 2, row)],
        [var a, var b, var c] when !char.IsNumber(a) && !char.IsNumber(b) && char.IsNumber(c) => [GetNumberRightSide(range.End.Value - 1, row)],
        [var a, var b, var c] when !char.IsNumber(a) && char.IsNumber(b) && !char.IsNumber(c) => [b - '0']
        };
        return res;
    }

    private static int GetNumberRightSide(int index, string row)
    {
        var numbers = new List<char>() { row[index] };
        index += 1;
        while (index < 140 && char.IsNumber(row[index]))
        {
            numbers.Add(row[index]);
            index += 1;
        }
        return int.Parse(new string(numbers.ToArray()));
    }

    private static int GetNumberLeftSide(int index, string row)
    {
        var numbers = new List<char>() { row[index] };
        index -= 1;
        while (index >= 0 && char.IsNumber(row[index]))
        {
            numbers.Add(row[index]);
            index -= 1;
        }
        numbers.Reverse();
        return int.Parse(new string(numbers.ToArray()));
    }
}