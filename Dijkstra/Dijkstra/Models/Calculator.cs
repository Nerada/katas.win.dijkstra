// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Dijkstra.Calculator.cs
// Created on: 20220906
// -----------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace Dijkstra.Models;

public static class Calculator
{
    public static IReadOnlySet<Point> ShortestPath(this Grid grid, Point startPoint, Point endPoint)
    {
        List<Point> currentPoints    = new();
        List<Point> calculatedPoints = new();

        startPoint.SetDistanceToEnd(endPoint);
        currentPoints.Add(startPoint);

        while (currentPoints.Any())
        {
            Point currentPoint = currentPoints.OrderBy(point => point.TotalDistance).First();

            // EndPoint found
            if (currentPoint.X == endPoint.X && currentPoint.Y == endPoint.Y) return GetRoute(currentPoint);

            calculatedPoints.Add(currentPoint);
            currentPoints.Remove(currentPoint);

            foreach (Point neighboringPoint in GetNeighboringPoints(grid, currentPoint, endPoint))
            {
                // Point already handled
                if (calculatedPoints.Any(x => x.X == neighboringPoint.X && x.Y == neighboringPoint.Y)) continue;

                if (!currentPoints.Any(x => x.X == neighboringPoint.X && x.Y == neighboringPoint.Y))
                {
                    // New point found
                    currentPoints.Add(neighboringPoint);
                    continue;
                }

                // Already processed point found in current points
                Point existingPoint = currentPoints.First(x => x.X == neighboringPoint.X && x.Y == neighboringPoint.Y);
                if (existingPoint.TotalDistance > currentPoint.TotalDistance)
                {
                    currentPoints.Remove(existingPoint);
                    currentPoints.Add(neighboringPoint);
                }
            }
        }

        throw new InvalidOperationException("Endpoint could not be found.");
    }

    private static HashSet<Point> GetRoute(Point endPoint)
    {
        HashSet<Point> route = new();

        Point backTrackPoint = endPoint;

        while (backTrackPoint.Origin != null)
        {
            route.Add(backTrackPoint);
            backTrackPoint = backTrackPoint.Origin;
        }

        route.Add(backTrackPoint);

        return route;
    }

    private static IReadOnlySet<Point> GetNeighboringPoints(Grid grid, Point currentPoint, Point targetPoint)
    {
        List<(int x, int y)> neighborLocations = new()
        {
            (currentPoint.X, currentPoint.Y - 1),
            (currentPoint.X, currentPoint.Y + 1),
            (currentPoint.X                 - 1, currentPoint.Y),
            (currentPoint.X                 + 1, currentPoint.Y)
        };

        neighborLocations.RemoveAll(location => location.x < 0 || location.x > grid.Size.X - 1);
        neighborLocations.RemoveAll(location => location.y < 0 || location.y > grid.Size.Y - 1);

        List<Point> gridPoints = neighborLocations.Select(location => grid.Points.Single(point => point.X == location.x && point.Y == location.y)).ToList();
        gridPoints.RemoveAll(point => point.Blocked);
        gridPoints.RemoveAll(point => point.Origin != null);

        void SetPointValues(Point point)
        {
            point.SetOrigin(currentPoint);
            point.SetDistanceFromStart(currentPoint.DistanceFromStart + 1);
            point.SetDistanceToEnd(targetPoint);
        }

        gridPoints.ForEach(SetPointValues);

        return gridPoints.ToHashSet();
    }
}