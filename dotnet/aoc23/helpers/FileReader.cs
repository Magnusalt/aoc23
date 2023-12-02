namespace aoc23.helpers
{
    public static class FileReader
    {
        public static Task<string[]> ReadAllLinesAsync(int day)
        {
            return File.ReadAllLinesAsync(@$"C:\src\aoc23\dotnet\aoc23\input\{day}.txt");
        }
        public static string[] ReadAllLines(int day)
        {
            return File.ReadAllLines(@$"C:\src\aoc23\dotnet\aoc23\input\{day}.txt");
        }
    }
}