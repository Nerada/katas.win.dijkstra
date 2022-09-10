// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Dijkstra.Extensions.cs
// Created on: 20220909
// -----------------------------------------------

using System;
using System.Collections.Generic;
using Dijkstra.Models;

namespace Dijkstra.Extensions;

public static class Extensions
{
    public static IReadOnlySet<Point> ShortestPath(this Grid grid, Point startPoint, Point endPoint, bool aStarEnabled = true) => Calculator.ShortestPath(grid, startPoint, endPoint, aStarEnabled);

    public static bool IsDirectNeighbor(this Point pointOrigin, Point? otherPoint)
    {
        if (otherPoint == null) return false;

        int xDiff = Math.Abs(pointOrigin.Coordinate.X - otherPoint.Coordinate.X);
        int yDiff = Math.Abs(pointOrigin.Coordinate.Y - otherPoint.Coordinate.Y);

        return xDiff <= 1 && yDiff <= 1 && xDiff != yDiff;
    }
}