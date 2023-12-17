using aoc23.helpers;

public static class Day15
{
    public static int RunPart1()
    {
        var input = FileReader.ReadAllLines(15);

        var test = Hash("HASH");

        var total = input[0].Split(',').Select(Hash).Sum();

        return total;
    }

    private static int Hash(string word)
    {
        int current = 0;
        foreach (var c in word)
        {
            current += c;
            current *= 17;
            current %= 256;
        }

        return current;
    }

    public static int RunPart2()
    {
        var input = FileReader.ReadAllLines(15);
        var instructions = input[0].Split(',');
        var boxMap = Enumerable.Range(0, 256).ToDictionary(k => k, _ => new LinkedList<string>());

        foreach (var instruction in instructions)
        {
            if (instruction.Contains('-'))
            {
                var label = instruction[..^1];
                var boxIndex = Hash(label);
                var toBeRemoved = boxMap[boxIndex].FirstOrDefault(l => l.StartsWith(label));

                if (toBeRemoved != null)
                {
                    _ = boxMap[boxIndex].Remove(toBeRemoved);
                }
            }
            else
            {
                var microInstructions = instruction.Split('=');
                var boxIndex = Hash(microInstructions[0]);
                var toBeReplaced = boxMap[boxIndex].FirstOrDefault(l => l.StartsWith(microInstructions[0]));

                var lens = $"{microInstructions[0]} {microInstructions[1]}";

                if (toBeReplaced != null)
                {
                    var node = boxMap[boxIndex].Find(toBeReplaced);
                    _ = boxMap[boxIndex].AddAfter(node!, lens);
                    _ = boxMap[boxIndex].Remove(toBeReplaced);
                }
                else
                {
                    boxMap[boxIndex].AddLast(lens);
                }
            }
        }

        var relevant = boxMap.Where(kv => kv.Value.Count > 0);

        var total = relevant.Select(kv => kv.Value.Select((l, i) => (kv.Key + 1) * int.Parse(l.Split(' ')[1]) * (i + 1)));

        return total.SelectMany(r => r).Sum();
    }
}