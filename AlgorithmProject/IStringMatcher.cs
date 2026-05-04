namespace AlgorithmProject;

public interface IStringMatcher
{
    string Name { get; }

    SearchResult Search(string text, string pattern);
}
