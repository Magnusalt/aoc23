using System.Numerics;
using aoc23.helpers;

public static class Day18
{
    public static int RunPart1()
    {
        var input = FileReader.ReadAllLines(18);

        var lines = new List<Line>();
        int x = 0;
        int y = 0;
        for (int i = 0; i < input.Length; i++)
        {
            var start = new Vector2(x, y);
            var inputLine = input[i].Split(' ');
            var dir = inputLine[0];
            var length = int.Parse(inputLine[1]);
            (x, y) = dir switch
            {
                "U" => (x: x, y: y - length),
                "D" => (x: x, y: y + length),
                "L" => (x: x - length, y: y),
                "R" => (x: x + length, y: y),
                _ => throw new Exception()
            };
            var end = new Vector2(x, y);
            lines.Add(new Line(start, end));
        }

        var minXLine = lines.MinBy(l => Math.Min(l.Start.X, l.End.X));
        var maxXLine = lines.MaxBy(l => Math.Max(l.Start.X, l.End.X));
        var minYLine = lines.MinBy(l => Math.Min(l.Start.Y, l.End.Y));
        var maxYLine = lines.MaxBy(l => Math.Max(l.Start.Y, l.End.Y));

        var minX = Math.Min(minXLine.Start.X, minXLine.End.X);
        var maxX = Math.Max(maxXLine.Start.X, maxXLine.End.X);
        var minY = Math.Min(minYLine.Start.Y, minYLine.End.Y);
        var maxY = Math.Max(maxYLine.Start.Y, maxYLine.End.Y);

        var guessedStart = (x: (int)(maxX - minX) / 2, y: (int)(maxY - minY) / 2);

        var visited = new HashSet<Vector2>();

        var queue = new Queue<Vector2>();
        queue.Enqueue(new Vector2(guessedStart.x, guessedStart.y));

        while (queue.TryDequeue(out var point))
        {
            if (visited.Contains(point))
            {
                continue;
            }
            if (lines.Any(l => l.IsOnLine(point)))
            {
                continue;
            }
            visited.Add(point);

            queue.Enqueue(new Vector2(point.X, point.Y - 1));
            queue.Enqueue(new Vector2(point.X, point.Y + 1));
            queue.Enqueue(new Vector2(point.X - 1, point.Y));
            queue.Enqueue(new Vector2(point.X + 1, point.Y));
        }
        var circumference = lines.Sum(l => l.Length);

        return visited.Count + circumference;
    }

    public static int RunPart2()
    {
        var input = FileReader.ReadAllLines(18);
        var hexCodes = input.Select(l => l.Split('#')[1][..^1]).Select(s => $"{ToDirection(s[^1])} {Convert.ToInt32(s[..^1], 16)}").ToList();

        var lines = new List<Line>();
        int x = 0;
        int y = 0;
        for (int i = 0; i < input.Length; i++)
        {
            var start = new Vector2(x, y);
            var inputLine = hexCodes[i].Split(' ');
            var dir = inputLine[0];
            var length = int.Parse(inputLine[1]);
            (x, y) = dir switch
            {
                "U" => (x: x, y: y - length),
                "D" => (x: x, y: y + length),
                "L" => (x: x - length, y: y),
                "R" => (x: x + length, y: y),
                _ => throw new Exception()
            };
            var end = new Vector2(x, y);
            lines.Add(new Line(start, end));
        }

        var minXLine = lines.MinBy(l => Math.Min(l.Start.X, l.End.X));
        var maxXLine = lines.MaxBy(l => Math.Max(l.Start.X, l.End.X));
        var minYLine = lines.MinBy(l => Math.Min(l.Start.Y, l.End.Y));
        var maxYLine = lines.MaxBy(l => Math.Max(l.Start.Y, l.End.Y));

        var minX = Math.Min(minXLine.Start.X, minXLine.End.X);
        var maxX = Math.Max(maxXLine.Start.X, maxXLine.End.X);
        var minY = Math.Min(minYLine.Start.Y, minYLine.End.Y);
        var maxY = Math.Max(maxYLine.Start.Y, maxYLine.End.Y);

        var guessedStart = (x: (int)(maxX - minX) / 2, y: (int)(maxY - minY) / 2);

        

        var visited = new HashSet<Vector2>();

        var queue = new Queue<Vector2>();
        queue.Enqueue(new Vector2(guessedStart.x, guessedStart.y));

        while (queue.TryDequeue(out var point))
        {
            if (visited.Contains(point))
            {
                continue;
            }
            if (lines.Any(l => l.IsOnLine(point)))
            {
                continue;
            }
            visited.Add(point);

            queue.Enqueue(new Vector2(point.X, point.Y - 1));
            queue.Enqueue(new Vector2(point.X, point.Y + 1));
            queue.Enqueue(new Vector2(point.X - 1, point.Y));
            queue.Enqueue(new Vector2(point.X + 1, point.Y));
        }
        var circumference = lines.Sum(l => l.Length);

        return visited.Count + circumference;
    }

    private static string ToDirection(char c)
    {
        return c switch
        {
            '0' => "R",
            '1' => "D",
            '2' => "L",
            '3' => "U",
            _ => throw new NotImplementedException()
        };
    }
    record Line(Vector2 Start, Vector2 End)
    {
        public bool IsOnLine(Vector2 point)
        {
            var dx = Start.X - End.X;
            var dy = Start.Y - End.Y;

            var dist1 = Vector2.Distance(Start, point);
            var dist2 = Vector2.Distance(point, End);
            var sum = dist1 + dist2;

            return Length == sum && ((dx == 0 && point.X == Start.X) || (dy == 0 && point.Y == Start.Y));
        }
        public int Length => (int)Vector2.Distance(Start, End);
    }
}