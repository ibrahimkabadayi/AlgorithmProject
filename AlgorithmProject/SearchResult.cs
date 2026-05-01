using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmProject;
public class SearchResult
{
    public List<int> Occurrences { get; set; } = new();

    public long Comparisons { get; set; }

    public double ElapsedMs { get; set; }
}
