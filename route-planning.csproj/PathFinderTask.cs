using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RoutePlanning
{
    public static class PathFinderTask
    {
        public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
        {
            var bestOrder = MakeTrivialPermutation(checkpoints.Length);
            var routes = new List<int[]>();
            CreateRoutes(routes, 1, bestOrder);
            Dictionary<int[], double> routesAndLength = new Dictionary<int[], double>();
            if (routes.Count > 0)
            {
                for (int i = 0; i < routes.Count; i++)
                {
                    if (!routesAndLength.ContainsKey(routes[i]))
                        routesAndLength[routes[i]] = PointExtensions.GetPathLength(checkpoints, routes[i]);
                    else continue;
                }
                var minDistance = routesAndLength.OrderBy(kvp => kvp.Value).First();
                bestOrder = minDistance.Key;
            }
            return bestOrder;
        }

        private static int[] MakeTrivialPermutation(int size)
        {
            var bestOrder = new int[size];

            return bestOrder;
        }

        public static void CreateRoutes(List<int[]> routes, int startIndex, int[] checkpointsOrder)
        {
            if (startIndex == checkpointsOrder.Length)
            {
                var clonedOrder = (int[])checkpointsOrder.Clone();
                routes.Add(clonedOrder);
                return;
            }
            for (int i = 1; i < checkpointsOrder.Length; i++)
            {
                bool found = false;
                for (int j = 0; j < startIndex; j++)
                {
                    if (checkpointsOrder[j] == i)
                    {
                        found = true;
                        break;
                    }
                }
                if (found)
                    continue;
                checkpointsOrder[startIndex] = i;
                CreateRoutes(routes, startIndex + 1, checkpointsOrder);
            }
        }
    }
}