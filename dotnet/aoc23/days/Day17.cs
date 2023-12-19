using aoc23.helpers;

public static class Day17
{
    public static int RunPart1()
    {

        var input = FileReader.ReadAllLines(17);

        var grid = CreateCharMatrix(input);
        var maxX = grid[0].Length - 1;
        var maxY = grid.Length - 1;
        var goal = new Position(maxX, maxY, Direction.Down, 0);
        var res = Dijikstra(grid, new Position(0, 0, Direction.Right, 1), goal, GetNeighbours);

        return res;
    }
    public static int RunPart2()
    {
        var input = FileReader.ReadAllLines(17);

        var grid = CreateCharMatrix(input);
        var maxX = grid[0].Length - 1;
        var maxY = grid.Length - 1;
        var goal = new Position(maxX, maxY, Direction.Down, 0);
        var res = Dijikstra(grid, new Position(0, 0, Direction.Right, 1), goal, GetNeighboursPart2);

        return res;
    }

    private static int Dijikstra(char[][] grid, Position start, Position goal, Func<char[][], Position, List<Position>> neighbours)
    {
        var queue = new PriorityQueue<Position, int>();
        var visited = new Dictionary<Position, int>();
        var path = new Dictionary<Position, Position>();

        queue.Enqueue(start, 0);
        var part2Start = new Position(0, 0, Direction.Down, 0);
        queue.Enqueue(part2Start, 0);
        visited.Add(start, 0);
        visited.Add(part2Start, 0);

        while (queue.TryDequeue(out var pos, out var priority))
        {
            if (pos.X == goal.X && pos.Y == goal.Y)
            {
                return priority;
            }

            foreach (var neighbour in neighbours(grid, pos))
            {
                var cost = AccumulatedHeat(pos.Direction, neighbour.Direction, visited[pos], (pos.X, pos.Y), (neighbour.X, neighbour.Y), grid);
                if (cost < (visited.TryGetValue(neighbour, out var neighCost) ? neighCost : int.MaxValue))
                {
                    visited[neighbour] = cost;
                    path.Add(neighbour, pos);
                    var heuristics = Math.Abs(goal.X - neighbour.X) + Math.Abs(goal.Y - neighbour.Y);
                    queue.Enqueue(neighbour, cost + heuristics);
                }
            }
        }
        return 0;
    }

    private static List<Position> GetNeighbours(char[][] grid, Position current)
    {
        var neighbours = new List<Position>();

        var (x, y, dir, l) = current;

        if (l < 3)
        {
            Position? forward = dir switch
            {
                Direction.Up when y > 0 => new(x, y - 1, dir, l + 1),
                Direction.Down when y < grid.Length - 1 => new(x, y + 1, dir, l + 1),
                Direction.Left when x > 0 => new(x - 1, y, dir, l + 1),
                Direction.Right when x < grid[0].Length - 1 => new(x + 1, y, dir, l + 1),
                _ => null
            };
            if (forward is not null)
            {
                neighbours.Add(forward);
            }
        }

        Position? leftDir = dir switch
        {
            Direction.Up when x > 0 => new(x - 1, y, Direction.Left, 1),
            Direction.Down when x < grid[0].Length - 1 => new(x + 1, y, Direction.Right, 1),
            Direction.Right when y > 0 => new(x, y - 1, Direction.Up, 1),
            Direction.Left when y < grid.Length - 1 => new(x, y + 1, Direction.Down, 1),
            _ => null
        };
        Position? rightDir = dir switch
        {
            Direction.Up when x < grid[0].Length - 1 => new(x + 1, y, Direction.Right, 1),
            Direction.Down when x > 0 => new(x - 1, y, Direction.Left, 1),
            Direction.Right when y < grid.Length - 1 => new(x, y + 1, Direction.Down, 1),
            Direction.Left when y > 0 => new(x, y - 1, Direction.Up, 1),
            _ => null
        };

        if (leftDir is not null)
        {
            neighbours.Add(leftDir);
        }
        if (rightDir is not null)
        {
            neighbours.Add(rightDir);
        }
        return neighbours;
    }

    private static List<Position> GetNeighboursPart2(char[][] grid, Position current)
    {
        var neighbours = new List<Position>();

        var (_, _, dir, l) = current;

        if (l < 4)
        {
            neighbours.AddRange(GetNeighboursInDirection(dir, grid, current));
            return neighbours;
        }

        var leftDir = dir switch
        {
            Direction.Up => GetNeighboursInDirection(Direction.Left, grid, current),
            Direction.Down => GetNeighboursInDirection(Direction.Right, grid, current),
            Direction.Right => GetNeighboursInDirection(Direction.Up, grid, current),
            Direction.Left => GetNeighboursInDirection(Direction.Down, grid, current),
            _ => []
        };
        var rightDir = dir switch
        {
            Direction.Up => GetNeighboursInDirection(Direction.Right, grid, current),
            Direction.Down => GetNeighboursInDirection(Direction.Left, grid, current),
            Direction.Right => GetNeighboursInDirection(Direction.Down, grid, current),
            Direction.Left => GetNeighboursInDirection(Direction.Up, grid, current),
            _ => []
        };

        neighbours.AddRange(leftDir);
        neighbours.AddRange(rightDir);

        return neighbours;
    }

    private static List<Position> GetNeighboursInDirection(Direction dir, char[][] grid, Position start)
    {
        var (x, y, _, _) = start;
        var neighbours = new List<Position>();
        for (int i = 4; i <= 10; i++)
        {
            Position? forward = dir switch
            {
                Direction.Up when y - i >= 0 => new(x, y - i, dir, i),
                Direction.Down when y + i <= grid.Length - 1 => new(x, y + i, dir, i),
                Direction.Left when x - i >= 0 => new(x - i, y, dir, i),
                Direction.Right when x + i <= grid[0].Length - 1 => new(x + i, y, dir, i),
                _ => null
            };
            if (forward is not null)
            {
                neighbours.Add(forward);
            }
        }
        return neighbours;
    }

    private static int AccumulatedHeat(Direction startDirection, Direction endDirection, int startHeat, (int x, int y) start, (int x, int y) end, char[][] grid)
    {
        var accumulatedHeat = startHeat;

        var length = (start, end) switch
        {
            (var s, var e) when s.x == e.x => Math.Abs(s.y - e.y),
            (var s, var e) when s.y == e.y => Math.Abs(s.x - e.x),
            _ => throw new Exception()
        };

        switch (startDirection, endDirection)
        {
            case (Direction.Up, Direction.Up):
                accumulatedHeat += MoveUp(start, length, grid);
                return accumulatedHeat;
            case (Direction.Down, Direction.Down):
                accumulatedHeat += MoveDown(start, length, grid);
                return accumulatedHeat;
            case (Direction.Left, Direction.Left):
                accumulatedHeat += MoveLeft(start, length, grid);
                return accumulatedHeat;
            case (Direction.Right, Direction.Right):
                accumulatedHeat += MoveRight(start, length, grid);
                return accumulatedHeat;

            case (Direction.Up, Direction.Left):
                accumulatedHeat += MoveLeft(start, length, grid);
                return accumulatedHeat;
            case (Direction.Down, Direction.Left):
                accumulatedHeat += MoveLeft(start, length, grid);
                return accumulatedHeat;
            case (Direction.Left, Direction.Up):
                accumulatedHeat += MoveUp(start, length, grid);
                return accumulatedHeat;
            case (Direction.Right, Direction.Up):
                accumulatedHeat += MoveUp(start, length, grid);
                return accumulatedHeat;

            case (Direction.Up, Direction.Right):
                accumulatedHeat += MoveRight(start, length, grid);
                return accumulatedHeat;
            case (Direction.Down, Direction.Right):
                accumulatedHeat += MoveRight(start, length, grid);
                return accumulatedHeat;
            case (Direction.Left, Direction.Down):
                accumulatedHeat += MoveDown(start, length, grid);
                return accumulatedHeat;
            case (Direction.Right, Direction.Down):
                accumulatedHeat += MoveDown(start, length, grid);
                return accumulatedHeat;
        }
        return 0;
    }

    private static int MoveUp((int x, int y) start, int length, char[][] grid)
    {
        var accumulatedHeat = 0;
        for (int i = start.y - 1; i > start.y - length - 1; i--)
        {
            accumulatedHeat += grid[i][start.x] - '0';
        }
        return accumulatedHeat;
    }
    private static int MoveDown((int x, int y) start, int length, char[][] grid)
    {
        var accumulatedHeat = 0;
        for (int i = start.y+1; i < start.y + length + 1; i++)
        {
            accumulatedHeat += grid[i][start.x] - '0';
        }
        return accumulatedHeat;
    }

    private static int MoveRight((int x, int y) start, int length, char[][] grid)
    {
        var accumulatedHeat = 0;
        for (int i = start.x + 1; i < start.x + length + 1; i++)
        {
            accumulatedHeat += grid[start.y][i] - '0';
        }
        return accumulatedHeat;
    }

    private static int MoveLeft((int x, int y) start, int length, char[][] grid)
    {
        var accumulatedHeat = 0;
        for (int i = start.x - 1; i > start.x - length - 1; i--)
        {
            accumulatedHeat += grid[start.y][i] - '0';
        }
        return accumulatedHeat;
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

    record Position(int X, int Y, Direction Direction, int Length);
    enum Direction
    {
        Up,
        Down,
        Right,
        Left
    }
}