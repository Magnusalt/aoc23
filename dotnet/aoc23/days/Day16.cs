using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using aoc23.helpers;

public static class Day16
{
    public static int RunPart1()
    {
        var input = FileReader.ReadAllLines(16);
        var grid = CreateCharMatrix(input);

        Position start = new(0, 0, Direction.Right);

        var total = FindBeamPath(grid, start);

        return total;
    }

    private static int FindBeamPath(char[][] grid, Position position)
    {
        HashSet<Position> energized = new HashSet<Position>();
        var queue = new Queue<Position>();
        queue.Enqueue(position);

        while (queue.Count > 0)
        {
            var pos = queue.Dequeue();

            if (pos.X < 0 || pos.X >= grid[0].Length || pos.Y < 0 || pos.Y >= grid.Length || energized.Contains(pos))
            {
                continue;
            }
            energized.Add(pos);
            var current = grid[pos.Y][pos.X];
            var dir = pos.Direction;

            if (current == '.')
            {
                switch (dir)
                {
                    case Direction.Up:
                        queue.Enqueue(new(pos.X, pos.Y - 1, dir));
                        break;
                    case Direction.Down:
                        queue.Enqueue(new(pos.X, pos.Y + 1, dir));
                        break;
                    case Direction.Left:
                        queue.Enqueue(new(pos.X - 1, pos.Y, dir));
                        break;
                    case Direction.Right:
                        queue.Enqueue(new(pos.X + 1, pos.Y, dir));
                        break;
                }
            }

            if (current == '/')
            {
                switch (dir)
                {
                    case Direction.Up:
                        queue.Enqueue(new(pos.X + 1, pos.Y, Direction.Right));
                        break;
                    case Direction.Down:
                        queue.Enqueue(new(pos.X - 1, pos.Y, Direction.Left));
                        break;
                    case Direction.Left:
                        queue.Enqueue(new(pos.X, pos.Y + 1, Direction.Down));
                        break;
                    case Direction.Right:
                        queue.Enqueue(new(pos.X, pos.Y - 1, Direction.Up));
                        break;
                }
            }

            if (current == '\\')
            {
                switch (dir)
                {
                    case Direction.Up:
                        queue.Enqueue(new(pos.X - 1, pos.Y, Direction.Left));
                        break;
                    case Direction.Down:
                        queue.Enqueue(new(pos.X + 1, pos.Y, Direction.Right));
                        break;
                    case Direction.Left:
                        queue.Enqueue(new(pos.X, pos.Y - 1, Direction.Up));
                        break;
                    case Direction.Right:
                        queue.Enqueue(new(pos.X, pos.Y + 1, Direction.Down));
                        break;
                }
            }

            if (current == '-')
            {
                switch (dir)
                {
                    case Direction.Up:
                    case Direction.Down:
                        queue.Enqueue(new(pos.X - 1, pos.Y, Direction.Left));
                        queue.Enqueue(new(pos.X + 1, pos.Y, Direction.Right));
                        break;
                    case Direction.Left:
                        queue.Enqueue(new(pos.X - 1, pos.Y, dir));
                        break;
                    case Direction.Right:
                        queue.Enqueue(new(pos.X + 1, pos.Y, dir));
                        break;
                }
            }

            if (current == '|')
            {
                switch (dir)
                {
                    case Direction.Up:
                        queue.Enqueue(new(pos.X, pos.Y - 1, dir));
                        break;
                    case Direction.Down:
                        queue.Enqueue(new(pos.X, pos.Y + 1, dir));
                        break;
                    case Direction.Left:
                    case Direction.Right:
                        queue.Enqueue(new(pos.X, pos.Y + 1, Direction.Down));
                        queue.Enqueue(new(pos.X, pos.Y - 1, Direction.Up));
                        break;
                }
            }
        }
        var total = energized.Select(p => (p.X, p.Y)).Distinct();

        return total.Count();
    }

    public static int RunPart2()
    {
        var input = FileReader.ReadAllLines(16);
        var grid = CreateCharMatrix(input);

        var max = 0;

        for (int i = 0; i < grid.Length; i++)
        {
            Position start = new(0, i, Direction.Right);
            var numberOfTiles = FindBeamPath(grid, start);
            max = numberOfTiles > max ? numberOfTiles : max;
        }
        for (int i = 0; i < grid.Length; i++)
        {

            Position start = new(grid[0].Length - 1, i, Direction.Left);
            var numberOfTiles = FindBeamPath(grid, start);
            max = numberOfTiles > max ? numberOfTiles : max;
        }
        for (int i = 0; i < grid[0].Length; i++)
        {
            Position start = new(i, 0, Direction.Down);
            var numberOfTiles = FindBeamPath(grid, start);
            max = numberOfTiles > max ? numberOfTiles : max;
        }
        for (int i = 0; i < grid[0].Length; i++)
        {
            Position start = new(i, grid.Length - 1, Direction.Up);
            var numberOfTiles = FindBeamPath(grid, start);
            max = numberOfTiles > max ? numberOfTiles : max;
        }


        return max;
    }

    private static char[][] CreateCharMatrix(string[] current)
    {
        var nbrOfRows = current.Length;
        var result = new char[nbrOfRows][];
        for (int y = 0; y < nbrOfRows; y++)
        {
            result[y] = current[y].ToCharArray();
        }
        return result;
    }

    struct Position(int x, int y, Direction direction)
    {
        public int X { get; } = x;
        public int Y { get; } = y;
        public Direction Direction { get; set; } = direction;
    }

    enum Direction
    {
        Up,
        Down,
        Right,
        Left
    }
}