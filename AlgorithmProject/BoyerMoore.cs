using System.Diagnostics;

namespace AlgorithmProject;

public class BoyerMoore : IStringMatcher
{
    public string Name => "Boyer-Moore";

    public Dictionary<char, int> BuildBadSymbolTable(string pattern)
    {
        int m = pattern.Length;
        var d1 = new Dictionary<char, int>();

        for (int i = 0; i < m - 1; i++)
            d1[pattern[i]] = m - 1 - i;

        return d1;
    }

    private static int BadSymbolShift(Dictionary<char, int> d1, char c, int m, int j)
    {
        int tableVal = d1.TryGetValue(c, out int v) ? v : m;
        int shift = tableVal - (m - 1 - j);
        return shift < 1 ? 1 : shift;
    }

    public int[] BuildGoodSuffixTable(string pattern)
    {
        int m = pattern.Length;
        int[] d2 = new int[m];
        int[] suffix = ComputeSuffix(pattern);

        for (int i = 0; i < m; i++)
            d2[i] = m;

        int p = 0;
        for (int j = m - 1; j >= 0; j--)
        {
            if (suffix[j] == j + 1)
            {
                for (; p < m - 1 - j; p++)
                    if (d2[p] == m)
                        d2[p] = m - 1 - j;
            }
        }

        for (int j = 0; j < m - 1; j++)
        {
            int s = suffix[j];
            if (s > 0)
            {
                int mismatchPos = m - 1 - s;
                d2[mismatchPos] = m - 1 - j;
            }
        }

        d2[m - 1] = 1;

        return d2;
    }

    private static int[] ComputeSuffix(string pattern)
    {
        int m = pattern.Length;
        int[] suffix = new int[m];
        suffix[m - 1] = m;

        int g = m - 1, f = 0;
        for (int i = m - 2; i >= 0; i--)
        {
            if (i > g && suffix[i + m - 1 - f] < i - g)
            {
                suffix[i] = suffix[i + m - 1 - f];
            }
            else
            {
                if (i < g) g = i;
                f = i;
                while (g >= 0 && pattern[g] == pattern[g + m - 1 - f])
                    g--;
                suffix[i] = f - g;
            }
        }
        return suffix;
    }

    public void PrintBadSymbolTable(string pattern)
    {
        int m = pattern.Length;
        var d1 = BuildBadSymbolTable(pattern);

        Console.WriteLine($"Bad-Symbol Table (Boyer-Moore) for pattern \"{pattern}\" (m={m}):");
        Console.WriteLine("  Character      d1 value");
        Console.WriteLine("  " + new string('-', 24));
        foreach (var kv in d1)
            Console.WriteLine($"  '{kv.Key}'          {kv.Value,10}");
        Console.WriteLine($"  (other)       {m,10}   <- default");
    }

    public void PrintGoodSuffixTable(string pattern)
    {
        int m = pattern.Length;
        int[] d2 = BuildGoodSuffixTable(pattern);

        Console.WriteLine($"Good-Suffix Table (Boyer-Moore) for pattern \"{pattern}\" (m={m}):");
        Console.WriteLine("  j (mismatch pos)     d2[j]");
        Console.WriteLine("  " + new string('-', 30));
        for (int j = 0; j < m; j++)
            Console.WriteLine($"  {j,-20} {d2[j],8}");
    }

    public SearchResult Search(string text, string pattern)
    {
        var result = new SearchResult();

        int n = text.Length;
        int m = pattern.Length;

        if (m == 0 || m > n)
            return result;

        var d1 = BuildBadSymbolTable(pattern);
        var d2 = BuildGoodSuffixTable(pattern);

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

                i += Math.Max(1, d2[0]);
            }
            else
            {
                int bsShift = BadSymbolShift(d1, text[i + j], m, j);
                int gsShift = d2[j];

                i += Math.Max(bsShift, gsShift);
            }
        }

        sw.Stop();
        result.ElapsedMs = sw.Elapsed.TotalMilliseconds;
        return result;
    }
}