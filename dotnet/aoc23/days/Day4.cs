using aoc23.helpers;

public class Day4
{
    public static int RunPart1()
    {
        var input = FileReader.ReadAllLines(4);
        var sum = 0;
        foreach (var item in input)
        {
            var split = item.Split('|', StringSplitOptions.TrimEntries);
            var winning = split[0].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Skip(2);
            var ticketNumbers = split[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var matches = ticketNumbers.Length - ticketNumbers.Except(winning).Count();
            var points = (int)Math.Pow(2, matches - 1);
            sum += points;
        }
        return sum;
    }
    public static int RunPart2()
    {
        var input = FileReader.ReadAllLines(4);
        var tickets = new Dictionary<int, (int copies, int newTicket)>();
        var index = 1;
        foreach (var item in input)
        {
            var split = item.Split('|', StringSplitOptions.TrimEntries);
            var winning = split[0].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Skip(2);
            var ticketNumbers = split[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var matches = ticketNumbers.Length - ticketNumbers.Except(winning).Count();
            tickets.Add(index, (1, matches));
            index++;
        }
        foreach (var item in tickets)
        {
            var start = item.Key + 1;
            var copiesToAdd = item.Value.copies;
            for (int i = start; i < start + item.Value.newTicket; i++)
            {
                var (copies, newTicket) = tickets[i];
                tickets[i] = (copies + copiesToAdd, newTicket);
            }
        }
        return tickets.Values.Sum(v => v.copies);
    }
}