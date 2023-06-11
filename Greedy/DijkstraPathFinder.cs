using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class DijkstraPathFinder
{
     
    
    public IEnumerable<PathWithCost> GetPathsByDijkstra(State state, Point start, IEnumerable<Point> targets)
    {
        var path = new Dictionary<Point, PathData>
        {
            {start, new PathData { PreviousPoint = new Point(int.MinValue, int.MinValue), TotalCost = 0 } } 
        };
        var visited = new HashSet<Point>();
        var currentPoint = start; // The point which we are opening
        var chests = targets.ToHashSet();

        while (true)
        {
            if (chests.Contains(currentPoint))
            {
                yield return GetPathWithCost(start, currentPoint, path);
                chests.Remove(currentPoint);
            }
            if(chests.Count == 0)
            {
                yield break;
            }

            var nextPossibleSteps = GetNextPossibleSteps(state, currentPoint, visited);
            if(nextPossibleSteps.Count == 0)
            {
                try
                {
                    currentPoint = path.Where(x => !visited.Contains(x.Key))
                        .OrderBy(x => x.Value.TotalCost)
                        .First().Key;
                }
                catch
                {
                    yield break;
                }
            }

            //adding new points to path
            foreach(var step in nextPossibleSteps.Where(x => !path.ContainsKey(x.Point)))
            {
                path.Add(step.Point, new PathData { PreviousPoint = currentPoint , TotalCost = int.MaxValue });
            }
            //updating costs
            foreach(var step in nextPossibleSteps)
            {
                var newCost = step.Cost + path[currentPoint].TotalCost;
                if (path[step.Point].TotalCost > newCost)
                {
                    path[step.Point].TotalCost = newCost;
                    path[step.Point].PreviousPoint = currentPoint;
                }
            }
            visited.Add(currentPoint);
            try
            {
                currentPoint = path.Where(x => !visited.Contains(x.Key))
                    .OrderBy(x => x.Value.TotalCost)
                    .First().Key;
            }
            catch
            {
                yield break;
            }

        }

    }  

    public HashSet<PointAndCost> GetNextPossibleSteps(State state, Point currentPoint, HashSet<Point> visited)
    {
        var steps = new Point[] { new Point(1,0), new Point(-1,0), new Point(0,1), new Point(0,-1) };
        var result = new HashSet<PointAndCost>();
        foreach(var step in steps)
        {
            var nextPoint = currentPoint + step;
            if(!state.InsideMap(nextPoint) || state.IsWallAt(nextPoint) || visited.Contains(nextPoint))
            {
                continue;
            }
            result.Add(new PointAndCost { Point = nextPoint, Cost = state.CellCost[nextPoint.X, nextPoint.Y] });
        }
        return result;
    }

    public PathWithCost GetPathWithCost(Point start, Point target, Dictionary<Point, PathData> path)
    {
        var cost = path[target].TotalCost;
        var points = new List<Point>();
        var currentPoint = target;
        while (currentPoint != new Point(int.MinValue, int.MinValue))
        {
            points.Add(currentPoint);
            currentPoint = path[currentPoint].PreviousPoint;
        }
        points.Reverse();
        return new PathWithCost(cost, points.ToArray());
    }
}

public class PointAndCost
{
    public Point Point { get; set; }
    public int Cost { get; set; }
}

public class PathData
{
    public int TotalCost { get; set; }
    public Point PreviousPoint { get; set; }
}