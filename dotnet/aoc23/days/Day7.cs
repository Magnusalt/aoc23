using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using aoc23.helpers;

public static class Day7
{
    public static long RunPart1()
    {
        var input = FileReader.ReadAllLines(7).Select(r => r.Split(' ', StringSplitOptions.TrimEntries)).Select(s => (hand: s[0], bid: int.Parse(s[1])));

        var comparer = new HandComparer1();

        var sum = input.OrderBy(x => x.hand, comparer).Aggregate((sum: 0L, index: 1), (s, v) =>
        {
            s.sum += (v.bid * s.index);
            s.index++;
            return s;
        });

        return sum.sum;
    }

    internal class HandComparer1 : IComparer<string>
    {
        const string values = "AKQJT98765432";
        public int Compare(string x, string y)
        {
            var firstHandStrength = GetStrength(x);
            var secondHandStrength = GetStrength(y);
            var handCompare = firstHandStrength.CompareTo(secondHandStrength);

            if (handCompare == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    var rankX = values.IndexOf(x[i]);
                    var rankY = values.IndexOf(y[i]);
                    var relative = rankX.CompareTo(rankY);
                    if (relative == 0)
                    {
                        continue;
                    }
                    return -1 * relative;
                }
            }
            return handCompare;
        }

        private static int GetStrength(string hand)
        {
            var grouped = hand.GroupBy(c => c).ToList();
            return grouped switch
            {
            [var g] => 7,
            [var g1, var g2] when (g1.Count() == 1 && g2.Count() == 4) || (g1.Count() == 4 && g2.Count() == 1) => 6,
            [var g1, var g2] when (g1.Count() == 2 && g2.Count() == 3) || (g1.Count() == 3 && g2.Count() == 2) => 5,
            [var g1, var g2, var g3] when (g1.Count() == 3 && g2.Count() == 1 && g3.Count() == 1) || (g1.Count() == 1 && g2.Count() == 3 && g3.Count() == 1) || (g1.Count() == 1 && g2.Count() == 1 && g3.Count() == 3) => 4,
            [var g1, var g2, var g3] when (g1.Count() == 2 && g2.Count() == 2 && g3.Count() == 1) || (g1.Count() == 1 && g2.Count() == 2 && g3.Count() == 2) || (g1.Count() == 2 && g2.Count() == 1 && g3.Count() == 2) => 3,
            [var g1, var g2, var g3, var g4] when (g1.Count() == 2 && g2.Count() == 1 && g3.Count() == 1 && g4.Count() == 1) || (g1.Count() == 1 && g2.Count() == 2 && g3.Count() == 1 && g4.Count() == 1) || (g1.Count() == 1 && g2.Count() == 1 && g3.Count() == 2 && g4.Count() == 1) || (g1.Count() == 1 && g2.Count() == 1 && g3.Count() == 1 && g4.Count() == 2) => 2,
            [_, _, _, _, _] => 1,
                _ => 0
            };
        }
    }

    public static int RunPart2()
    {
        return 0;
    }
    internal class HandComparer2 : IComparer<string>
    {
        const string values = "AKQT98765432J";

        public int Compare(string? x, string? y)
        {
            throw new NotImplementedException();
        }
    }
}