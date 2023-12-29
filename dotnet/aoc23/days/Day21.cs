using aoc23.helpers;

public static class Day21
{
    public static int RunPart1()
    {

        var input = FileReader.ReadAllLines(21);

        var matrix = Transformers.CreateCharMatrix(input);

        var visited = new HashSet<StepCounter>();
        var start = FindStart(matrix);
        var queue = new Stack<StepCounter>();
        queue.Push(start);

        var numberOfSteps = 64;

        while (queue.TryPop(out var current))
        {
            if (!visited.Add(current))
            {
                continue;
            }

            if (current.Steps == numberOfSteps)
            {
                continue;
            }

            if (TryGetPlotAbove(matrix, current, out var above))
            {
                queue.Push(above);
            }
            if (TryGetPlotBelow(matrix, current, out var below))
            {
                queue.Push(below);
            }
            if (TryGetPlotLeft(matrix, current, out var left))
            {
                queue.Push(left);
            }
            if (TryGetPlotRight(matrix, current, out var right))
            {
                queue.Push(right);
            }
        }

        return visited.Count(n=> n.Steps == numberOfSteps);
    }

    private static bool TryGetPlotRight(char[][] matrix, StepCounter current, out StepCounter right)
    {
        if (current.X == matrix[0].Length - 1 || matrix[current.Y][current.X + 1] == '#')
        {
            right = current;
            return false;
        }
        right = new (current.X + 1, current.Y, current.Steps + 1);
        return true;
    }

    private static bool TryGetPlotLeft(char[][] matrix, StepCounter current, out StepCounter left)
    {
        if (current.X == 0 || matrix[current.Y][current.X - 1] == '#')
        {
            left = current;
            return false;
        }
        left = new (current.X - 1, current.Y, current.Steps + 1);
        return true;
    }

    private static bool TryGetPlotBelow(char[][] matrix, StepCounter current, out StepCounter below)
    {
        if (current.Y == matrix.Length - 1 || matrix[current.Y + 1][current.X] == '#')
        {
            below = current;
            return false;
        }
        below = new (current.X, current.Y + 1, current.Steps + 1);
        return true;
    }

    private static bool TryGetPlotAbove(char[][] matrix, StepCounter current, out StepCounter above)
    {
        if (current.Y == 0 || matrix[current.Y - 1][current.X] == '#')
        {
            above = current;
            return false;
        }
        above = new (current.X, current.Y - 1, current.Steps + 1);
        return true;
    }

    private static StepCounter FindStart(char[][] matrix)
    {
        for (int i = 0; i < matrix.Length; i++)
        {
            for (int j = 0; j < matrix[0].Length; j++)
            {
                if (matrix[i][j] == 'S')
                {
                    return new (j, i, 0);
                }
            }
        }
        throw new Exception("not found");
    }
    record StepCounter(int X, int Y, int Steps);

    record StepCounter2(int X, int Y, int Steps, int Up, int Down, int Left, int Right) : StepCounter(X, Y, Steps);
}