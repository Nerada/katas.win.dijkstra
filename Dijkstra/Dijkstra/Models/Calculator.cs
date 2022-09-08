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
        List<Point> activePoints     = new();
        List<Point> calculatedPoints = new();

        startPoint.SetDistanceToEnd(endPoint.Coordinate);
        activePoints.Add(startPoint);

        while (activePoints.Any())
        {
            Point currentPoint = activePoints.OrderBy(point => point.TotalDistance).First();

            // EndPoint found
            if (currentPoint.Equals(endPoint)) return GetRoute(currentPoint);

            calculatedPoints.Add(currentPoint);
            activePoints.Remove(currentPoint);

            foreach (Point neighboringPoint in GetNeighboringPoints(grid, currentPoint))
            {
                // Point already handled
                if (calculatedPoints.Any(calcPoint => calcPoint.Equals(neighboringPoint))) continue;

                if (!activePoints.Any(activePoint => activePoint.Equals(neighboringPoint)))
                {
                    // New point found
                    activePoints.Add(neighboringPoint);

                    neighboringPoint.SetOrigin(currentPoint);
                    neighboringPoint.SetDistanceFromStart(currentPoint.DistanceFromStart + 1);
                    neighboringPoint.SetDistanceToEnd(endPoint.Coordinate);

                    continue;
                }

                // Already processed point found in current points
                Point existingPoint = activePoints.First(activePoint => activePoint.Equals(neighboringPoint));
                if (existingPoint.TotalDistance > currentPoint.TotalDistance)
                {
                    activePoints.Remove(existingPoint);
                    activePoints.Add(neighboringPoint);
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

    private static IReadOnlySet<Point> GetNeighboringPoints(Grid grid, Point currentPoint)
    {
        List<Coordinate> neighborCoordinates = new()
        {
            currentPoint.Coordinate with {Y = currentPoint.Coordinate.Y - 1},
            currentPoint.Coordinate with {Y = currentPoint.Coordinate.Y + 1},
            currentPoint.Coordinate with {X = currentPoint.Coordinate.X - 1},
            currentPoint.Coordinate with {X = currentPoint.Coordinate.X + 1}
        };

        neighborCoordinates.RemoveAll(location => location.X < 0 || location.X > grid.Size.X - 1);
        neighborCoordinates.RemoveAll(location => location.Y < 0 || location.Y > grid.Size.Y - 1);

        List<Point> gridPoints = neighborCoordinates.Select(neighborCoordinate => grid.Points.Single(point => point.Coordinate.Equals(neighborCoordinate))).ToList();
        gridPoints.RemoveAll(point => point.Blocked);

        return gridPoints.ToHashSet();
    }
}