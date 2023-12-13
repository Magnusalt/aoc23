using aoc23.helpers;

public static class Day13
{
    public static int RunPart1()
    {
        var input = FileReader.ReadAllLines(13);

        var allGroups = new List<List<string>>();
        var group = new List<string>();
        foreach (var item in input)
        {
            if (string.IsNullOrEmpty(item))
            {
                allGroups.Add(group);
                group = new List<string>();
            }
            else
            {
                group.Add(item);
            }
        }
        allGroups.Add(group);

        var colSum = 0;
        var rowSum = 0;

        foreach (var current in allGroups)
        {
            var nbrOfCols = current[0].Length;
            var foundVertical = false;
            for (int i = 1; i < nbrOfCols; i++)
            {
                var prev = new string(current.Select(s => s[i - 1]).ToArray());
                var next = new string(current.Select(s => s[i]).ToArray());

                if (prev == next)
                {
                    foundVertical = TestForColumnReflection(current, i);
                    colSum += foundVertical ? i : 0;
                    if (foundVertical)
                    {
                        break;
                    }
                }
            }
            if (!foundVertical)
            {
                for (int i = 1; i < current.Count; i++)
                {
                    var prev = current[i - 1];
                    var next = current[i];
                    if (prev == next)
                    {
                        var foundHorizontal = TestForRowReflection(current, i);
                        rowSum += foundHorizontal ? i : 0;
                        if (foundHorizontal)
                        {
                            break;
                        }
                    }
                }
            }
        }

        return colSum + 100 * rowSum;
    }

    private static bool TestForRowReflection(List<string> current, int i)
    {
        var aboveIndex = i - 1;
        var belowIndex = i;
        while (aboveIndex >= 0 && belowIndex < current.Count)
        {
            var prev = current[aboveIndex];
            var next = current[belowIndex];
            if (prev != next)
            {
                return false;
            }
            aboveIndex--;
            belowIndex++;
        }
        return true;
    }

    private static bool TestForColumnReflection(List<string> current, int i)
    {
        var leftIndex = i - 1;
        var rightIndex = i;
        while (leftIndex >= 0 && rightIndex < current[0].Length)
        {
            var prev = new string(current.Select(s => s[leftIndex]).ToArray());
            var next = new string(current.Select(s => s[rightIndex]).ToArray());
            if (prev != next)
            {
                return false;
            }
            leftIndex--;
            rightIndex++;
        }
        return true;
    }

    public static int RunPart2()
    {
        var input = FileReader.ReadAllLines(13);

        var allGroups = new List<List<string>>();
        var group = new List<string>();
        foreach (var item in input)
        {
            if (string.IsNullOrEmpty(item))
            {
                allGroups.Add(group);
                group = new List<string>();
            }
            else
            {
                group.Add(item);
            }
        }
        allGroups.Add(group);

        var colSum = 0;
        var rowSum = 0;

        foreach (var current in allGroups)
        {
            var nbrOfCols = current[0].Length;
            var nbrOfRows = current.Count;

            for (int y = 0; y < nbrOfRows; y++)
            {
                for (int x = 0; x < nbrOfCols; x++)
                {
                    var charMatrix = CreateCharMatrix(current);
                    charMatrix[y][x] = charMatrix[y][x] == '#' ? '.' : '#';
                    var foundVertical = false;
                    for (int i = 1; i < nbrOfCols; i++)
                    {
                        var prev = new string(charMatrix.Select(s => s[i - 1]).ToArray());
                        var next = new string(charMatrix.Select(s => s[i]).ToArray());

                        if (prev == next)
                        {
                            var originalReflection = TestForColumnReflection(current, i);
                            foundVertical = TestForColumnReflection(charMatrix, i) && !originalReflection;
                            colSum += foundVertical ? i : 0;
                            if (foundVertical)
                            {
                                goto EndOfLoop;
                            }
                        }
                    }
                    if (!foundVertical)
                    {
                        for (int i = 1; i < current.Count; i++)
                        {
                            var prev = new string(charMatrix[i - 1]);
                            var next = new string(charMatrix[i]);
                            if (prev == next)
                            {
                                var originalReflection = TestForRowReflection(current, i);
                                var foundHorizontal = TestForRowReflection(charMatrix, i) && !originalReflection;
                                rowSum += foundHorizontal ? i : 0;
                                if (foundHorizontal)
                                {
                                    goto EndOfLoop;
                                }
                            }
                        }
                    }
                }
            }
        EndOfLoop:
            continue;
        }

        return colSum + 100 * rowSum;
    }
    private static bool TestForRowReflection(char[][] current, int i)
    {
        var aboveIndex = i - 1;
        var belowIndex = i;
        while (aboveIndex >= 0 && belowIndex < current.Length)
        {
            var prev = new string(current[aboveIndex]);
            var next = new string(current[belowIndex]);
            if (prev != next)
            {
                return false;
            }
            aboveIndex--;
            belowIndex++;
        }
        return true;
    }

    private static bool TestForColumnReflection(char[][] current, int i)
    {
        var leftIndex = i - 1;
        var rightIndex = i;
        while (leftIndex >= 0 && rightIndex < current[0].Length)
        {
            var prev = new string(current.Select(s => s[leftIndex]).ToArray());
            var next = new string(current.Select(s => s[rightIndex]).ToArray());
            if (prev != next)
            {
                return false;
            }
            leftIndex--;
            rightIndex++;
        }
        return true;
    }
    private static char[][] CreateCharMatrix(List<string> current)
    {
        var nbrOfCols = current[0].Length;
        var nbrOfRows = current.Count;
        var result = new char[nbrOfRows][];
        for (int y = 0; y < nbrOfRows; y++)
        {
            result[y] = new char[nbrOfCols];
            for (int x = 0; x < nbrOfCols; x++)
            {
                result[y][x] = current[y][x];
            }
        }
        return result;
    }
}