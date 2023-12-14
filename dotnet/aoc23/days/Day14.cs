using System.Formats.Asn1;
using System.Net.Mail;
using System.Security.Cryptography;
using aoc23.helpers;

public static class Day14
{
    public static int RunPart1()
    {
        var input = FileReader.ReadAllLines(14);


        var matrix = CreateCharMatrix(input);

        for (var y = 0; y < matrix.Length; y++)
        {
            for (int x = 0; x < matrix[y].Length; x++)
            {
                if (matrix[y][x] == '.')
                {
                    var tempY = y + 1;
                    while (tempY < matrix.Length && matrix[tempY][x] == '.')
                    {
                        tempY++;
                    }
                    if (tempY < matrix.Length && matrix[tempY][x] == 'O')
                    {
                        matrix[tempY][x] = '.';
                        matrix[y][x] = 'O';
                    }
                }
            }
        }

        var total = matrix.Select((r, i) => r.Where(c => c == 'O').Count() * (matrix.Length - i));


        return total.Sum();
    }

    public static int RunPart2()
    {
        var input = FileReader.ReadAllLines(14);

        var matrix = CreateCharMatrix(input);

        var period = 0;
        var before = HashMatrix(matrix);
        var after = 0;
        var mem = new Dictionary<int, List<int>> { [before] = [period] };
        while (true)
        {
            matrix = TiltNorth(matrix);
            matrix = TiltWest(matrix);
            matrix = TiltSouth(matrix);
            matrix = TiltEast(matrix);
            after = HashMatrix(matrix);

            period++;
            if (mem.ContainsKey(after))
            {
                mem[after].Add(period);
            }
            else
            {
                mem.Add(after, [period]);
            }
            if (period == 200)
            {
                break;
            }
        }

        var index = 0;
        foreach (var item in mem)
        {
            if (item.Value.Count > 1)
            {
                break;
            }
            index++;
        }

        var periodLength = mem.Count - index;

        var allCycles = 1_000_000_000;

        var cycling = allCycles - index;

        var mod = cycling % periodLength;

        var matrix2 = CreateCharMatrix(input);

        for (int i = 0; i < mod + index; i++)
        {
            matrix2 = TiltNorth(matrix2);
            matrix2 = TiltWest(matrix2);
            matrix2 = TiltSouth(matrix2);
            matrix2 = TiltEast(matrix2);
        }

        var total = matrix2.Select((r, i) => r.Where(c => c == 'O').Count() * (matrix.Length - i));

        return total.Sum();
    }

    private static int HashMatrix(char[][] matrix)
    {
        var chars = matrix.SelectMany(r => r).ToArray();

        return GetDeterministicHashCode(new string(chars));
    }

    static int GetDeterministicHashCode(this string str)
    {
        unchecked
        {
            int hash1 = (5381 << 16) + 5381;
            int hash2 = hash1;

            for (int i = 0; i < str.Length; i += 2)
            {
                hash1 = ((hash1 << 5) + hash1) ^ str[i];
                if (i == str.Length - 1)
                    break;
                hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
            }

            return hash1 + (hash2 * 1566083941);
        }
    }

    private static char[][] TiltNorth(char[][] matrix)
    {
        for (var y = 0; y < matrix.Length; y++)
        {
            for (int x = 0; x < matrix[y].Length; x++)
            {
                if (matrix[y][x] == '.')
                {
                    var tempY = y + 1;
                    while (tempY < matrix.Length && matrix[tempY][x] == '.')
                    {
                        tempY++;
                    }
                    if (tempY < matrix.Length && matrix[tempY][x] == 'O')
                    {
                        matrix[tempY][x] = '.';
                        matrix[y][x] = 'O';
                    }
                }
            }
        }

        return matrix;
    }

    private static char[][] TiltSouth(char[][] matrix)
    {
        for (var y = matrix.Length - 1; y >= 0; y--)
        {
            for (int x = 0; x < matrix[y].Length; x++)
            {
                if (matrix[y][x] == '.')
                {
                    var tempY = y - 1;
                    while (tempY >= 0 && matrix[tempY][x] == '.')
                    {
                        tempY--;
                    }
                    if (tempY >= 0 && matrix[tempY][x] == 'O')
                    {
                        matrix[tempY][x] = '.';
                        matrix[y][x] = 'O';
                    }
                }
            }
        }

        return matrix;
    }

    private static char[][] TiltWest(char[][] matrix)
    {
        for (var x = 0; x < matrix[0].Length; x++)
        {
            for (int y = 0; y < matrix.Length; y++)
            {
                if (matrix[y][x] == '.')
                {
                    var tempX = x + 1;
                    while (tempX < matrix[y].Length && matrix[y][tempX] == '.')
                    {
                        tempX++;
                    }
                    if (tempX < matrix[y].Length && matrix[y][tempX] == 'O')
                    {
                        matrix[y][tempX] = '.';
                        matrix[y][x] = 'O';
                    }
                }
            }
        }

        return matrix;
    }

    private static char[][] TiltEast(char[][] matrix)
    {
        for (var x = matrix[0].Length - 1; x >= 0; x--)
        {
            for (int y = 0; y < matrix.Length; y++)
            {
                if (matrix[y][x] == '.')
                {
                    var tempX = x - 1;
                    while (tempX >= 0 && matrix[y][tempX] == '.')
                    {
                        tempX--;
                    }
                    if (tempX >= 0 && matrix[y][tempX] == 'O')
                    {
                        matrix[y][tempX] = '.';
                        matrix[y][x] = 'O';
                    }
                }
            }
        }

        return matrix;
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
}