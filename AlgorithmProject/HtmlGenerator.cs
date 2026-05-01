using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmProject;

public class HtmlGenerator
{
    private static readonly Random _rng = new(42);   // fixed seed → reproducible

    // ------------------------------------------------------------------
    // Type 2: Random bit-string HTML
    // ------------------------------------------------------------------
    /// <summary>
    /// Generates an HTML file whose body is a random sequence of '0' and '1'
    /// characters totalling approximately <paramref name="targetBytes"/> bytes.
    /// </summary>
    public static void GenerateBitStringHtml(string path, int targetBytes)
    {
        int bodyLen = targetBytes - 200;    // subtract HTML overhead
        if (bodyLen < 1000) bodyLen = 1000;

        var bits = new StringBuilder(bodyLen);
        for (int i = 0; i < bodyLen; i++)
            bits.Append(_rng.Next(2) == 0 ? '0' : '1');

        string html =
                $"""
                    <!DOCTYPE html>
                    <html><head><meta charset="UTF-8"><title>Random Bit String</title></head>
                    <body>
                    {bits}
                    </body></html>
                    """;
        File.WriteAllText(path, html, Encoding.UTF8);
    }

    // ------------------------------------------------------------------
    // Type 1: English text HTML
    // ------------------------------------------------------------------
    private static readonly string[] _words =
    {
            "algorithm", "analysis", "pattern", "matching", "string", "text",
            "computer", "science", "experiment", "performance", "comparison",
            "brute", "force", "horspool", "boyer", "moore", "heuristic",
            "suffix", "prefix", "shift", "table", "character", "comparison",
            "occurrence", "search", "index", "complexity", "worst", "best",
            "average", "case", "time", "space", "memory", "input", "output",
            "file", "report", "result", "highlight", "mark", "html", "body",
            "data", "structure", "preprocessing", "window", "mismatch", "match",
            "overlap", "random", "english", "length", "frequent", "infrequent"
        };

    /// <summary>
    /// Generates an HTML file with pseudo-random English-like text totalling
    /// approximately <paramref name="targetBytes"/> bytes.
    /// </summary>
    public static void GenerateEnglishHtml(string path, int targetBytes)
    {
        int bodyLen = targetBytes - 400;
        if (bodyLen < 1000) bodyLen = 1000;

        var sb = new StringBuilder(bodyLen + 400);
        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html><head><meta charset=\"UTF-8\"><title>English Text</title></head>");
        sb.AppendLine("<body>");
        sb.AppendLine("<h1>Algorithm Analysis Text</h1>");

        int written = 0;
        while (written < bodyLen)
        {
            sb.Append("<p>");
            // Each paragraph: ~80 words
            for (int w = 0; w < 80 && written < bodyLen; w++)
            {
                string word = _words[_rng.Next(_words.Length)];
                if (w == 0) word = char.ToUpper(word[0]) + word[1..];
                sb.Append(word);
                written += word.Length;
                if (w < 79) { sb.Append(' '); written++; }
            }
            sb.AppendLine(".</p>");
            written += 5;
        }

        sb.AppendLine("</body></html>");
        File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
    }
}
