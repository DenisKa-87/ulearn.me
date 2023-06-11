using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

// Каждый документ — это список токенов. То есть List<string>.
// Вместо этого будем использовать псевдоним DocumentTokens.
// Это поможет избежать сложных конструкций:
// вместо List<List<string>> будет List<DocumentTokens>
using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism;

public class LevenshteinCalculator
{
    public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
    {
        var result = new List<ComparisonResult>();
        for (int i = 0; i < documents.Count - 1; i++)
        {
            for (int j = i + 1; j < documents.Count; j++)
            {
                result.Add(GetLevensteinDistance(documents[i], documents[j]));
            }
        }

        return result;
    }

    private ComparisonResult GetLevensteinDistance(DocumentTokens list1, DocumentTokens list2)
    {
        var opt = new double[list1.Count + 1, list2.Count + 1];
        for (int k = 0; k <= list1.Count; k++)
            opt[k, 0] = k;
        for (int k = 0; k <= list2.Count; k++)
            opt[0, k] = k;
        for (int i = 1; i <= list1.Count; i++)
        {
            for (int j = 1; j <= list2.Count; j++)
            {
                var tokDist = TokenDistanceCalculator.GetTokenDistance(list1[i - 1],
                    list2[j - 1]);

                if (tokDist == 0)
                {
                    opt[i, j] = opt[i - 1, j - 1];
                }
                else
                {
                    opt[i, j] = Math.Min(opt[i - 1, j - 1] + tokDist,
                        Math.Min(opt[i, j - 1] + 1, opt[i - 1, j] + 1));
                }
            }
        }
        return new
            ComparisonResult(list1, list2, opt[list1.Count, list2.Count]);
    }
}