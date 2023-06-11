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
		var opt = new int[first.Count, second.Count];
		for (int i = 0; i < first.Count; i++) 
			opt[i,0] = 0;
        for (int i = 0; i < second.Count; i++)
            opt[0,i] = 0;
		for(int i  = 1; i < first.Count; i++)
		{
			for(int j = 1; j < second.Count; j++)
			{
				if (first[i] == second[j])
				{
					opt[i, j] = opt[i - 1, j - 1] + 1;
				}
				else
				{
					opt[i, j] = Math.Max(opt[i, j - 1], opt[i, j - 1]);
				}
			}
		}
		return opt;
    }

	private static List<string> RestoreAnswer(int[,] opt, List<string> first, List<string> second)
	{
		var optHeight = opt.GetLength(0);
		var optWidth = opt.GetLength(1);


        var result = new List<string>();
		for(int i = optWidth - 1; i > 0; i--)
		{
			for(int j = optHeight - 1; j > 0; j--)
			{
				if (first[i] == second[j])
				{
					result.Add(first[i]);
					i--;
				}
			}
		}
		result.Reverse();
		return result;
	}
}