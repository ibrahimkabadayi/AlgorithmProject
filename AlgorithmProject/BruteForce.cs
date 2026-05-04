using System.Diagnostics;

namespace AlgorithmProject;

public class BruteForce : IStringMatcher
{
    public string Name => "Brute-Force";

    public SearchResult Search(string text, string pattern)
    {
        var result = new SearchResult();

        int n = text.Length;
        int m = pattern.Length;

        if (m == 0 || m > n)
            return result;

        var sw = Stopwatch.StartNew();

        for (int i = 0; i <= n - m; i++)
        {
            int j = 0;
            while (j < m)
            {
                result.Comparisons++;
                if (text[i + j] == pattern[j])
                    j++;
                else
                    break;
            }

            if (j == m)                 
                result.Occurrences.Add(i);

            
        }

        sw.Stop();
        result.ElapsedMs = sw.Elapsed.TotalMilliseconds;
        return result;
    }
}
