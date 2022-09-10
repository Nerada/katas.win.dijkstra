// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Dijkstra.Calculator.cs
// Created on: 20220909
// -----------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Dijkstra.Extensions;

namespace Dijkstra.Models;

public static class Calculator
{
    public static IReadOnlySet<Point> ShortestPath(Grid grid, Point startPoint, Point endPoint, bool aStarEnabled = true)
    {
        List<Point> activePoints     = new();
        List<Point> calculatedPoints = new();

        startPoint.SetDistanceToEnd(endPoint.Coordinate);
        activePoints.Add(startPoint);

        while (activePoints.Any())
        {
            Point currentPoint = activePoints.OrderByDescending(point => point.TotalDistance).Last();

            // EndPoint found
            if (currentPoint.Equals(endPoint)) return GetRoute(currentPoint);

            calculatedPoints.Add(currentPoint);
            activePoints.Remove(currentPoint);

            foreach (Coordinate neighboringCoordinate in GetNeighboringCoordinates(grid, currentPoint.Coordinate))
            {
                Point neighboringPoint = grid.PointAtCoordinate(neighboringCoordinate);
                if (neighboringPoint.Blocked) continue;

                // Point already handled
                if (calculatedPoints.Any(calcPoint => calcPoint.Equals(neighboringPoint))) continue;

                if (!activePoints.Any(activePoint => activePoint.Equals(neighboringPoint)))
                {
                    // New point found
                    activePoints.Add(neighboringPoint);

                    neighboringPoint.SetOrigin(currentPoint);
                    if (aStarEnabled) neighboringPoint.SetDistanceToEnd(endPoint.Coordinate);
                    continue;
                }

                // Active point found in current points
                Point currentPointNeighbor = activePoints.First(activePoint => activePoint.Equals(neighboringPoint));

                if (currentPointNeighbor.DistanceFromStart > currentPoint.DistanceFromStart + 1) activePoints.Remove(currentPointNeighbor);

                if (currentPointNeighbor.DistanceFromStart == currentPoint.DistanceFromStart + 1 && currentPointNeighbor.IsDirectNeighbor(currentPoint)) currentPointNeighbor.SetOrigin(currentPoint);
            }

            currentPoint.Visit();
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

    private static IReadOnlyList<Coordinate> GetNeighboringCoordinates(Grid grid, Coordinate currentCoordinate)
    {
        List<Coordinate> neighbors = new()
        {
            // Direct (+)
            new Coordinate(currentCoordinate.X - 1, currentCoordinate.Y),
            new Coordinate(currentCoordinate.X + 1, currentCoordinate.Y),
            new Coordinate(currentCoordinate.X,     currentCoordinate.Y - 1),
            new Coordinate(currentCoordinate.X,     currentCoordinate.Y + 1),

            // Diagonals (X)
            new Coordinate(currentCoordinate.X - 1, currentCoordinate.Y - 1),
            new Coordinate(currentCoordinate.X - 1, currentCoordinate.Y + 1),
            new Coordinate(currentCoordinate.X + 1, currentCoordinate.Y - 1),
            new Coordinate(currentCoordinate.X + 1, currentCoordinate.Y + 1)
        };

        neighbors.RemoveAll(coordinate => coordinate.X < 0 || coordinate.X > grid.Size.X - 1);
        neighbors.RemoveAll(coordinate => coordinate.Y < 0 || coordinate.Y > grid.Size.Y - 1);

        return neighbors;
    }
}