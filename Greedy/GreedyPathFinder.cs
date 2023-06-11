using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class GreedyPathFinder : IPathFinder
{
	public List<Point> FindPathToCompleteGoal(State state)
	{
		var paths = new DijkstraPathFinder();
		var startingPoint = state.Position;
		var chests = state.Chests;
		var collectedChests = new List<List<Point>>();
		if (chests.Count() < state.Goal)
			return new List<Point>();
		while (state.Goal != collectedChests.Count)
		{
			var pathAndCost = paths.GetPathsByDijkstra(state, startingPoint, chests).FirstOrDefault();
            if (pathAndCost ==  null)
            {
				return new List<Point>();
            }     
			if (state.Energy < pathAndCost.Cost)
			{
                return new List<Point>();
			}
            var points = pathAndCost.Path;
			points.RemoveAt(0);
            startingPoint = points.LastOrDefault();
			chests.Remove(startingPoint);
            collectedChests.Add(points);
		}
		return collectedChests.SelectMany(x => x).ToList();
	}
}