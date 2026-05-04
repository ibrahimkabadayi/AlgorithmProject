using System.Text;

namespace AlgorithmProject;

public static class HtmlHighlighter
{
public static string Highlight(string text, List<int> positions, int patternLength)
{
    if (positions.Count == 0)
        return WrapInPage(EscapeText(text));

    positions.Sort();
    var intervals = MergeIntervals(positions, patternLength);

    var sb = new StringBuilder(text.Length + intervals.Count * 15);
    int prev = 0;
    foreach (var (start, end) in intervals)
    {
        sb.Append(text, prev, start - prev);
        sb.Append("<mark>");
        sb.Append(text, start, end - start);
        sb.Append("</mark>");
        prev = end;
    }
    sb.Append(text, prev, text.Length - prev);

    return WrapInPage(sb.ToString());
}

private static List<(int start, int end)> MergeIntervals(List<int> positions, int len)
{
    var merged = new List<(int, int)>();
    int curStart = positions[0];
    int curEnd = positions[0] + len;

    for (int i = 1; i < positions.Count; i++)
    {
        int s = positions[i];
        int e = s + len;
        if (s < curEnd)
            curEnd = Math.Max(curEnd, e);
        else
        {
            merged.Add((curStart, curEnd));
            curStart = s;
            curEnd = e;
        }
    }
    merged.Add((curStart, curEnd));
    return merged;
}

private static string EscapeText(string text) => text;

private static string WrapInPage(string body)
{
    return "<!DOCTYPE html>\n<html lang=\"en\">\n<head>\n" +
           "  <meta charset=\"UTF-8\">\n" +
           "  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n" +
           "  <title>String Matching Output</title>\n" +
           "  <style>\n" +
           "    mark { background-color: #FFD700; color: #000; border-radius: 2px; }\n" +
           "    body { font-family: monospace; white-space: pre-wrap; word-break: break-all; padding: 1em; }\n" +
           "  </style>\n" +
           "</head>\n<body>\n" +
           body +
           "\n</body>\n</html>";
}
}
