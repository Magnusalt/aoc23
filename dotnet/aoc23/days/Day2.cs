using aoc23.helpers;

namespace aoc23.days;

static class Day2
{
    public static async Task<int> RunPart1()
    {
        var input = await FileReader.ReadAllLinesAsync(2);
        var result1 = input.Select(CreateGame).Where(ValidateGame).Sum(g => g.Id);
        return result1;
    }

    public static async Task<int> RunPart2()
    {
        var input = await FileReader.ReadAllLinesAsync(2);
        var result2 = input.Select(CreateGame).Select(FindSetPower).Sum();
        return result2;
    }
    private static int FindSetPower(Game game)
    {
        var grouped = game.CubeReveals.SelectMany(g => g).GroupBy(g => g.Color);
        return grouped.Aggregate(1, (a, g) => a * g.MaxBy(c => c.Count).Count);
    }

    private static Game CreateGame(string input)
    {
        var idSplit = input.Split(':');
        var id = int.Parse(idSplit[0].Split(' ')[1]);

        var reveals = idSplit[1].Split(';', StringSplitOptions.TrimEntries);
        var game = new Game(id, []);
        var i = 0;

        foreach (var reveal in reveals)
        {
            game.CubeReveals.Add([]);
            var cubesShown = reveal.Split(',', StringSplitOptions.TrimEntries);
            foreach (var cube in cubesShown)
            {
                var cubeInfo = cube.Split(' ');
                game.CubeReveals[i].Add(new Cube(cubeInfo[1], int.Parse(cubeInfo[0])));
            }
            i++;
        }
        return game;
    }

    private static bool ValidateGame(Game game)
    {
        var res = game.CubeReveals.SelectMany(g => g).All(ValidateCubeCount);
        return res;
    }

    private static bool ValidateCubeCount(Cube cube)
    {
        var res = cube.Color switch
        {
            "red" => cube.Count <= 12,
            "green" => cube.Count <= 13,
            "blue" => cube.Count <= 14
        };
        return res;
    }

    private record Game(int Id, List<List<Cube>> CubeReveals);

    private record Cube(string Color, int Count);
}