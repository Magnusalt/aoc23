using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using aoc23.helpers;

public static class Day7
{
    public static int RunPart1()
    {
        var input = FileReader.ReadAllLines(7).Select(r => r.Split(' ', StringSplitOptions.TrimEntries)).Select(s => (hand: s[0], bid: int.Parse(s[1])));

        input.Order()


        return 0;
    }

    public class HandComparer : IComparer<(string hand, int bid)>
    {
        public int Compare((string hand, int bid) x, (string hand, int bid) y)
        {
            if(GetStrength(x.hand) > GetStrength(y.hand)){
                
            }
        }

        public int GetStrength(string hand)
        {
            var grouped = hand.GroupBy(c => c).ToList();
            return grouped switch
            {
            [var g] => 7,
            [var g1, var g2] when (g1.Count() == 1 && g2.Count() == 4) || (g1.Count() == 4 && g2.Count() == 1) => 6,
            [var g1, var g2] when (g1.Count() == 2 && g2.Count() == 3) || (g1.Count() == 3 && g2.Count() == 2) => 5,
            [var g1, var g2, var g3] when (g1.Count() == 3 && g2.Count() == 1 && g3.Count() == 1) || (g1.Count() == 1 && g2.Count() == 3 && g3.Count() == 1) || (g1.Count() == 1 && g2.Count() == 1 && g3.Count() == 3) => 4,
            [var g1, var g2, var g3] when (g1.Count() == 2 && g2.Count() == 2 && g3.Count() == 1) || (g1.Count() == 1 && g2.Count() == 2 && g3.Count() == 2) || (g1.Count() == 2 && g2.Count() == 1 && g3.Count() == 2) => 3,
            [var g1, var g2, var g3, var g4] when (g1.Count() == 2 && g2.Count() == 1 && g3.Count() == 1 && g4.Count() == 1) || (g1.Count() == 1 && g2.Count() == 2 && g3.Count() == 1 && g4.Count() == 1) || (g1.Count() == 1 && g2.Count() == 1 && g3.Count() == 2 && g3.Count() == 1) || (g1.Count() == 1 && g2.Count() == 1 && g3.Count() == 1 && g3.Count() == 2) => 2,
            [var g1, var g2, var g3, var g4, var g5] when g1.Count() == 1 && g2.Count() == 1 && g3.Count() == 1 && g4.Count() == 1 && g5.Count() == 1  => 1
            _ => 0
            };
        }
    }

    public static int RunPart2()
    {
        return 0;
    }
}