using System.Security.Cryptography;

static class Day1
{
    public static async Task<int> RunPart1()
    {
        var input = await File.ReadAllLinesAsync(@"C:\src\aoc23\dotnet\aoc23\input\1.txt");

        var sum = 0;
        foreach (var line in input)
        {
            var first = '0';
            var last = '0';
            var foundFirst = false;
            foreach (var c in line)
            {
                if (char.IsNumber(c))
                {
                    if (!foundFirst)
                    {
                        first = c;
                        foundFirst = true;
                    }
                    last = c;
                }
            }
            sum += int.Parse(new string([first, last]));
        }

        return sum;
    }

    public static int RunPart2()
    {
        var input = File.ReadAllLines(@"C:\src\aoc23\dotnet\aoc23\input\1.txt");

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