using System;
using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class NotGreedyPathFinder : IPathFinder
{
    public List<Point> FindPathToCompleteGoal(State state)
    {
        // a path between two goals
        Dictionary<Tuple<Point, Point>, List<Point>> detailedPath = new Dictionary<Tuple<Point, Point>, 
            List<Point>>();
        //amount energy the player will lose between two certain points 
        Dictionary<Tuple<Point, Point>, int> routesCost = new Dictionary<Tuple<Point, Point>, int>();
        var route = GetAllPossibleRoutes(state, detailedPath, routesCost);
        List<Point> result = GetDetailedPath(route, detailedPath, routesCost);
        return result;
    }

    private List<Point> GetDetailedPath(List<Point> route,
        Dictionary<Tuple<Point, Point>, List<Point>> detailedPath, Dictionary<Tuple<Point, Point>, int> routesCost)
    {
        var result = new List<Point>();
        for (int i = 0; i < route.Count - 1; i++)
        {
            var twoPoints = Tuple.Create(route[i], route[i + 1]);
            var lst = new List<Point>(detailedPath[twoPoints]);
            lst.RemoveAt(0);
            result.AddRange(lst);
        }
        return result;
    }

    private List<Point> GetAllPossibleRoutes(State state, 
        Dictionary<Tuple<Point, Point>, List<Point>> detailedPath,
        Dictionary<Tuple<Point, Point>, int> routesCost)
    {
        var goals = state.Chests.ToList();
        var start = state.Position;
        var routes = new List<List<Point>>(); 
        var currentRoute = new List<Point>();
        currentRoute.Add(start);
        int energy = state.InitialEnergy;
        CreateAllPossibleRoutes(state, goals, routes, currentRoute, energy, detailedPath,routesCost);
        return routes.OrderByDescending(x => x.Count).FirstOrDefault();
    }

    private void CreateAllPossibleRoutes(State state, List<Point> goals, 
        List<List<Point>> routes, List<Point> currentRoute, int previousEnergy,
        Dictionary<Tuple<Point, Point>, List<Point>> detailedPath,
        Dictionary<Tuple<Point, Point>, int> routesCost)
    {
        int pathCost;
        var chestsCollected = new HashSet<Point>();
        int energy = previousEnergy;
        if (goals.Count == 0)
        {
            routes.Add(currentRoute);
            return;
        }
        for (int i = 0; i < goals.Count; i++)
        {
            energy = previousEnergy; // need this to be the same for every recursion branch
            var twoPoints = Tuple.Create(currentRoute[currentRoute.Count - 1], goals[i]);
            if (routesCost.ContainsKey(twoPoints))
            {
                pathCost = routesCost[twoPoints];
            }
            else
            {
                pathCost = CalculatePathCostAndChests(energy, state, 
                    currentRoute[currentRoute.Count - 1], goals[i], chestsCollected,
                    detailedPath, routesCost);
            }
            if (energy >= pathCost)
            {
                energy -= pathCost;
            }
            else
            {
                var stopRoute = new List<Point>(currentRoute); 
                routes.Add(stopRoute);
                break;
            }
            var nextGoalsList = new List<Point>(goals);
            nextGoalsList.RemoveAt(i);
            var nextRoute = new List<Point>(currentRoute)
            {
                goals[i]
            };
            CreateAllPossibleRoutes(state, nextGoalsList, routes, nextRoute, energy, detailedPath, routesCost);
        }
    }

    private static int CalculatePathCostAndChests(int energy,State state, Point start, 
        Point finish, HashSet<Point> chestsCollected,
        Dictionary<Tuple<Point, Point>, List<Point>> detailedPath,
        Dictionary<Tuple<Point, Point>, int> routesCost)
    {
        var pathFinder = new DijkstraPathFinder();
        int pathCost;
        var chestsOnTheRoute = new List<Point>();
        PathWithCost? pathAndCost;
        var twoPoints = Tuple.Create(start, finish);
        var path = new List<Point>();
        pathAndCost = pathFinder.GetPathsByDijkstra(state, start, new List<Point>() { finish })
                .FirstOrDefault();
        if (pathAndCost == null)
        {
            pathCost = int.MaxValue;
            path = null;
            chestsOnTheRoute = null;
        }
        else
        {
            pathCost = pathAndCost.Cost;
            path = pathAndCost.Path;
            chestsOnTheRoute = pathAndCost.Path.Select(x => x)
                .Where(x => state.Chests.Contains(x) && !chestsCollected.Contains(x) && x != pathAndCost.Start)
                .ToList();
            chestsCollected.UnionWith(chestsOnTheRoute);
        }
        if (!routesCost.ContainsKey(twoPoints))
        {   
            routesCost.Add(twoPoints, pathCost);
            detailedPath.Add(twoPoints, path);
        }
        else
        {
            routesCost[twoPoints] = pathCost;
            detailedPath[twoPoints] = path;
        }
        if (path == null)
        {
            return int.MaxValue;
        }
        return pathCost;
    }
}
