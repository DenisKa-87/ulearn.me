using System;
using System.Collections.Generic;

namespace Antiplagiarism;

public static class LongestCommonSubsequenceCalculator
{
    public static List<string> Calculate(List<string> first, List<string> second)
    {
        var opt = CreateOptimizationTable(first, second);
        return RestoreAnswer(opt, first, second);
    }

    private static int[,] CreateOptimizationTable(List<string> first, List<string> second)
    {
        var opt = new int[first.Count + 1, second.Count +1]; //because 0th column and row represent empty subsequences
        for (int i = 0; i <= first.Count; i++)
            opt[i, 0] = 0;
        for (int i = 0; i <= second.Count; i++)
            opt[0, i] = 0;
        for (int i = 1; i <= first.Count; i++)
        {
            for (int j = 1; j <= second.Count; j++)
            {
                if (first[i-1] == second[j-1])
                {
                    opt[i, j] = opt[i - 1, j - 1] + 1;
                }
                else
                {
                    opt[i, j] = Math.Max(opt[i, j - 1], opt[i-1, j]);
                }
            }
        }
        return opt;
    }

    private static List<string> RestoreAnswer(int[,] opt, List<string> first, List<string> second)
    {
        var result = new List<string>();
        int width = second.Count;
        int height = first.Count;
        while ((width > 0 && height > 0))
        {
            if (opt[height, width] == 0)
                break;
            if (first[height - 1] == second[width - 1])
            {
                result.Add(first[height - 1]);
                width--;
                height--;
            }
            else if (opt[height, width - 1] > opt[height - 1, width])
            {
                width--;
            }
            else
            {
                height--;
            }
        }
        result.Reverse();
        return result;
    }
}