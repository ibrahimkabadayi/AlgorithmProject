using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmProject;

internal class Horspool : IStringMatcher
{
    public string Name => "Horspool";

    public Dictionary<char, int> BuildBadCharTable(string pattern)
    {
        int m = pattern.Length;
        var table = new Dictionary<char, int>();

        for (int i = 0; i < m - 1; i++)
            table[pattern[i]] = m - 1 - i;

        return table;
    }

    private static int GetShift(Dictionary<char, int> table, char c, int m)
        => table.TryGetValue(c, out int s) ? s : m;

    public void PrintBadCharTable(string pattern)
    {
        int m = pattern.Length;
        var table = BuildBadCharTable(pattern);

        Console.WriteLine($"Bad-Symbol Table (Horspool) for pattern \"{pattern}\" (m={m}):");
        Console.WriteLine("  Character      Shift");
        Console.WriteLine("  " + new string('-', 20));

        foreach (var kv in table)
            Console.WriteLine($"  '{kv.Key}'          {kv.Value,6}");

        Console.WriteLine($"  (other)       {m,5}");
    }

    public SearchResult Search(string text, string pattern)
    {
        var result = new SearchResult();

        int n = text.Length;
        int m = pattern.Length;

        if (m == 0 || m > n)
            return result;

        var table = BuildBadCharTable(pattern);

        var sw = Stopwatch.StartNew();

        int i = 0;
        while (i <= n - m)
        {
            int j = m - 1;
            while (j >= 0)
            {
                result.Comparisons++;
                if (pattern[j] == text[i + j])
                    j--;
                else
                    break;
            }

            if (j < 0)
            {
                result.Occurrences.Add(i);
                i++;
            }
            else
            {
                int shift = GetShift(table, text[i + m - 1], m);
                i += shift;
            }
        }

        sw.Stop();
        result.ElapsedMs = sw.Elapsed.TotalMilliseconds;
        return result;
    }
}
