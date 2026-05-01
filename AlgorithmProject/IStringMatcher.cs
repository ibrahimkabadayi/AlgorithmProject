using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmProject;

public interface IStringMatcher
{
    string Name { get; }

    SearchResult Search(string text, string pattern);
}
