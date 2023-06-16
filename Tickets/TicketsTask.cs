using System;
using System.Numerics;

namespace Tickets;
public static class TicketsTask
{
    public static BigInteger Solve(int halfLen, int totalSum)
    {
        if (totalSum % 2 != 0)
            return 0;
        totalSum /= 2;
        var opt = CreateTable(halfLen, totalSum);
        CalculateTableValues(halfLen, totalSum, opt);
        return opt[totalSum, halfLen - 1] * opt[totalSum, halfLen - 1];
    }

    public static BigInteger[,] CreateTable(int halfLen, int totalSum)
    {
        var opt = new BigInteger[totalSum + 1, halfLen];
        for (int i = 0; i < halfLen; i++) 
            opt[0, i] = 1;
        for (int i = 0; i <= totalSum; i++)
        {
            if (i < 10)
                opt[i, 0] = 1;
            else opt[i, 0] = 0;
        }
        return opt;
    }

    public static void CalculateTableValues(int halfLen, int totalSum, BigInteger[,] opt)
    {
        int optWidth = opt.GetLength(1);
        int optHeight = opt.GetLength(0);
        for (int i = 1; i < optWidth; i++)
        {
            for (int j = 1; j < optHeight; j++)
            {
                for (int l = 0; l < 10; l++)
                {
                    if (j - l < 0) break;
                    opt[j, i] += opt[j - l, i - 1];
                }
            }
        }
    }
}
