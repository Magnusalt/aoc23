using System.Security.Cryptography;
using System.Xml.Schema;
using aoc23.helpers;
using Microsoft.VisualBasic;

public static class Day10
{
    public static int RunPart1()
    {
        var input = FileReader.ReadAllLines(10);

        var maxY = input.Length;
        var maxX = input[0].Length;

        var x = 0;
        var y = 0;

        for (int i = 0; i < maxY; i++)
        {
            for (int j = 0; j < maxX; j++)
            {
                if (input[i][j] == 'S')
                {
                    x = j;
                    y = i;
                    break;
                }
            }
            if (x != 0)
            {
                break;
            }
        }

        List<bool> nextPossibleDirection =
        [
            ("|7F".Contains(input[y - 1][x])),
            "-7J".Contains(input[y][x + 1]),
            "|LJ".Contains(input[y + 1][x]),
            "-LF".Contains(input[y][x - 1])
        ];

        var firstStep = nextPossibleDirection.IndexOf(true);
        Direction direction;
        (x, y, direction) = firstStep switch
        {
            0 => (x, y - 1, Direction.North),
            1 => (x + 1, y, Direction.East),
            2 => (x, y + 1, Direction.South),
            3 => (x - 1, y, Direction.West),
            _ => throw new Exception("invalid")
        };
        var count = 1;
        while (input[y][x] != 'S')
        {
            (x, y, direction) = (input[y][x], direction) switch
            {
                ('|', Direction.North) => (x, y - 1, Direction.North),
                ('|', Direction.South) => (x, y + 1, Direction.South),
                ('-', Direction.East) => (x + 1, y, Direction.East),
                ('-', Direction.West) => (x - 1, y, Direction.West),
                ('L', Direction.South) => (x + 1, y, Direction.East),
                ('L', Direction.West) => (x, y - 1, Direction.North),
                ('J', Direction.East) => (x, y - 1, Direction.North),
                ('J', Direction.South) => (x - 1, y, Direction.West),
                ('7', Direction.East) => (x, y + 1, Direction.South),
                ('7', Direction.North) => (x - 1, y, Direction.West),
                ('F', Direction.North) => (x + 1, y, Direction.East),
                ('F', Direction.West) => (x, y + 1, Direction.South),
                _ => throw new Exception("invalid")
            };
            count++;
        }


        return count / 2;
    }

    public static int RunPart2()
    {
        var input = FileReader.ReadAllLines(10);

        var translatedX = input.Select(i => string.Join('+', i.ToCharArray())).ToList();
        var yFiller = new string('+', translatedX[0].Length);
        var translatedGrid = new List<string>();

        foreach (var item in translatedX)
        {
            translatedGrid.Add(item);
            translatedGrid.Add(yFiller);
        }

        translatedGrid = translatedGrid[..^1];

        var maxY = translatedGrid.Count;
        var maxX = translatedGrid[0].Length;

        var x = 0;
        var y = 0;

        for (int i = 0; i < maxY; i++)
        {
            for (int j = 0; j < maxX; j++)
            {
                if (translatedGrid[i][j] == 'S')
                {
                    x = j;
                    y = i;
                    break;
                }
            }
            if (x != 0)
            {
                break;
            }
        }
        var mainLoop = new HashSet<(int x, int y, char underlying)> { (x, y, 'S') };

        List<bool> nextPossibleDirection =
        [
            ("|7F".Contains(translatedGrid[y - 2][x])),
            "-7J".Contains(translatedGrid[y][x + 2]),
            "|LJ".Contains(translatedGrid[y + 2][x]),
            "-LF".Contains(translatedGrid[y][x - 2])
        ];

        var firstStep = nextPossibleDirection.IndexOf(true);
        Direction direction;
        (x, y, direction) = firstStep switch
        {
            0 => (x, y - 1, Direction.North),
            1 => (x + 1, y, Direction.East),
            2 => (x, y + 1, Direction.South),
            3 => (x - 1, y, Direction.West),
            _ => throw new Exception("invalid")
        };

        while (translatedGrid[y][x] != 'S')
        {
            mainLoop.Add((x, y, translatedGrid[y][x]));
            (x, y, direction) = (translatedGrid[y][x], direction) switch
            {
                ('|', Direction.North) => (x, y - 1, Direction.North),
                ('|', Direction.South) => (x, y + 1, Direction.South),
                ('-', Direction.East) => (x + 1, y, Direction.East),
                ('-', Direction.West) => (x - 1, y, Direction.West),
                ('L', Direction.South) => (x + 1, y, Direction.East),
                ('L', Direction.West) => (x, y - 1, Direction.North),
                ('J', Direction.East) => (x, y - 1, Direction.North),
                ('J', Direction.South) => (x - 1, y, Direction.West),
                ('7', Direction.East) => (x, y + 1, Direction.South),
                ('7', Direction.North) => (x - 1, y, Direction.West),
                ('F', Direction.North) => (x + 1, y, Direction.East),
                ('F', Direction.West) => (x, y + 1, Direction.South),
                ('+', Direction.North) => (x, y - 1, Direction.North),
                ('+', Direction.East) => (x + 1, y, Direction.East),
                ('+', Direction.South) => (x, y + 1, Direction.South),
                ('+', Direction.West) => (x - 1, y, Direction.West),
                _ => throw new Exception("invalid")
            };
        }

        var outside = new HashSet<(int x, int y, char underlying)>();

        var perimeter = new List<(int x, int y)>();
        perimeter.AddRange(Enumerable.Range(0, maxX).Select(i => (i, 0)));
        perimeter.AddRange(Enumerable.Range(0, maxX).Select(i => (i, maxY - 1)));
        perimeter.AddRange(Enumerable.Range(0, maxY).Select(i => (0, i)));
        perimeter.AddRange(Enumerable.Range(0, maxY).Select(i => (maxX - 1, i)));

        foreach (var seed in perimeter)
        {
            var queue = new Queue<(int x, int y)>();
            queue.Enqueue(seed);

            while (queue.Count > 0)
            {
                var currentPos = queue.Dequeue();
                var underlying = translatedGrid[currentPos.y][currentPos.x];
                var currentNode = (currentPos.x, currentPos.y, underlying);
                if (mainLoop.Contains(currentNode) || outside.Contains(currentNode))
                {
                    continue;
                }
                outside.Add(currentNode);
                if (currentPos.x > 0)
                {
                    queue.Enqueue((currentPos.x - 1, currentPos.y));
                }
                if (currentPos.x < maxX - 1)
                {
                    queue.Enqueue((currentPos.x + 1, currentPos.y));
                }
                if (currentPos.y > 0)
                {
                    queue.Enqueue((currentPos.x, currentPos.y - 1));
                }
                if (currentPos.y < maxY - 1)
                {
                    queue.Enqueue((currentPos.x, currentPos.y + 1));
                }
            }
        }

        var outsideCount = outside.Where(o => o.underlying != '+').Count();
        var mainLoopCount = mainLoop.Where(o => o.underlying != '+').Count();

        var total = input.Length * input[0].Length;

        return total - outsideCount - mainLoopCount;
    }
    private enum Direction
    {
        North,
        East,
        South,
        West
    }
}