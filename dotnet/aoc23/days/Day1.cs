using aoc23.helpers;

namespace aoc23.days;

static class Day1
{
    public static async Task<int> RunPart1()
    {
        return (await FileReader.ReadAllLinesAsync(1))
        .Aggregate(0, (a, l) => a + (l.First(char.IsNumber) - '0') * 10 + (l.Last(char.IsNumber) - '0'));
    }

    public static int RunPart2()
    {
        var input = FileReader.ReadAllLines(1);

        var sum = 0;
        foreach (var line in input)
        {
            var spanLine = line.AsSpan();
            var first = '0';
            var last = '0';
            var foundFirst = false;

            for (int i = 0; i < spanLine.Length; i++)
            {
                var slice = spanLine[i..];
                var number = slice switch
                {
                [var c, ..] when char.IsNumber(c) => c,
                ['o', 'n', 'e', ..] => '1',
                ['t', 'w', 'o', ..] => '2',
                ['t', 'h', 'r', 'e', 'e', ..] => '3',
                ['f', 'o', 'u', 'r', ..] => '4',
                ['f', 'i', 'v', 'e', ..] => '5',
                ['s', 'i', 'x', ..] => '6',
                ['s', 'e', 'v', 'e', 'n', ..] => '7',
                ['e', 'i', 'g', 'h', 't', ..] => '8',
                ['n', 'i', 'n', 'e', ..] => '9',
                    _ => '0'
                };

                if (number > '0')
                {
                    if (!foundFirst)
                    {
                        first = number;
                        foundFirst = true;
                    }
                    last = number;
                }
            }

            sum += int.Parse(new string([first, last]));
        }

        return sum;
    }
}