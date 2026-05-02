namespace AlgorithmProject;

public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== String Matching Algorithm Comparison ===");
        Console.WriteLine("CSE 2046/2246 - Analysis of Algorithms\n");

        RunCorrectnessTests();

        Console.WriteLine("\n--- Interactive Mode ---");
        Console.Write("Enter path to HTML file (or press Enter to skip): ");
        string? filePath = Console.ReadLine()?.Trim();

        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
        {
            Console.Write("Enter pattern to search: ");
            string pattern = Console.ReadLine() ?? "";
            if (!string.IsNullOrEmpty(pattern))
            {
                RunAllAlgorithms(filePath, pattern);
            }
        }
        else if (!string.IsNullOrEmpty(filePath))
        {
            Console.WriteLine("File not found.");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    static void RunCorrectnessTests()
    {
        Console.WriteLine("=== CORRECTNESS TEST CASES ===\n");

        var testCases = new List<(string name, string text, string pattern)>
            {
                ("Test Case 1: Single Match",
                 "<HTML><BODY>WHICH_FINALLY_HALTS. _ _ AT_THAT POINT </BODY></HTML>",
                 "AT_THAT"),
                ("Test Case 2: Multiple Matches",
                 "<HTML><BODY>ABC_AT_THAT_XYZ_AT_THAT_END</BODY></HTML>",
                 "AT_THAT"),
                ("Test Case 3: Overlapping Matches",
                 "<HTML><BODY>AAAAAA</BODY></HTML>",
                 "AAA"),
                ("Test Case 4: No Match",
                 "<HTML><BODY>THIS_IS_A_TEST</BODY></HTML>",
                 "XYZ")
            };

        foreach (var (name, text, pattern) in testCases)
        {
            Console.WriteLine($"{'=',1}{'=',60}");
            Console.WriteLine(name);
            Console.WriteLine($"Text:    \"{text}\"");
            Console.WriteLine($"Pattern: \"{pattern}\"");
            Console.WriteLine();

            // Write temp files for output
            string outputDir = "test_outputs";
            Directory.CreateDirectory(outputDir);

            RunAllAlgorithmsOnText(text, pattern, name, outputDir);
        }
    }

    static void RunAllAlgorithmsOnText(string text, string pattern, string testName, string outputDir)
    {
        var algorithms = new IStringMatcher[]
        {
                new BruteForce(),
                new Horspool(),
                new BoyerMoore()
        };

        // Print tables for BM (and Horspool bad char table)
        var horspool = new Horspool();
        Console.WriteLine("--- Bad Symbol (Bad Character) Table ---");
        horspool.PrintBadCharTable(pattern);

        var bm = new BoyerMoore();
        Console.WriteLine("\n--- Good Suffix Table ---");
        bm.PrintGoodSuffixTable(pattern);
        Console.WriteLine();

        foreach (var algo in algorithms)
        {
            var result = algo.Search(text, pattern);
            Console.WriteLine($"[{algo.Name}]");
            Console.WriteLine($"  Occurrences           : {result.Occurrences.Count}");
            Console.WriteLine($"  Positions             : [{string.Join(", ", result.Occurrences)}]");
            Console.WriteLine($"  Character Comparisons : {result.Comparisons}");
            Console.WriteLine($"  Running Time          : {result.ElapsedMs:F4} ms");
            Console.WriteLine();
        }

        // Generate highlighted HTML using BruteForce results (all should match)
        var bf = new BruteForce();
        var bfResult = bf.Search(text, pattern);
        string highlighted = HtmlHighlighter.Highlight(text, bfResult.Occurrences, pattern.Length);
        string safeName = string.Join("_", testName.Split(Path.GetInvalidFileNameChars()));
        File.WriteAllText(Path.Combine(outputDir, safeName + "_output.html"), highlighted);
        Console.WriteLine($"  Highlighted HTML saved to: {outputDir}/{safeName}_output.html");
        Console.WriteLine();
    }

    static void RunAllAlgorithms(string filePath, string pattern)
    {
        Console.WriteLine($"\nReading file: {filePath}");
        string text = File.ReadAllText(filePath);
        Console.WriteLine($"File size: {text.Length:N0} characters\n");

        string outputDir = "search_outputs";
        Directory.CreateDirectory(outputDir);

        RunAllAlgorithmsOnText(text, pattern, Path.GetFileNameWithoutExtension(filePath), outputDir);
    }
}
