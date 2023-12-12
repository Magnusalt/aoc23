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

            var arrangement = split[0];
            var grouping = split[1].Split(',').Select(int.Parse).ToList();

            var groupPointer = 0;

            for (int i = 0; i < arrangement.Length; i++)
            {
                var current = arrangement[i];
                var groupLength = grouping[groupPointer];

                if (current == '#' || current == '?')
                {
                    var substring = arrangement.Substring(i, groupLength);
                    groupPointer++;
                    if (substring.All(c => c == '#' || c == '?'))
                    {
                        
                    }
                }

            }

        }


        return total;
    }

    public static int RunPart2()
    {
        var input = FileReader.ReadAllLines(12);

        var total = 0;


        return total;
    }

}