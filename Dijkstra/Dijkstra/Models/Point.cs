// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Dijkstra.Point.cs
// Created on: 20220906
// -----------------------------------------------

using System;

namespace Dijkstra.Models;

public readonly record struct Coordinate(int X, int Y);

public class Point
{
    public Point(Coordinate coordinate)
    {
        Coordinate = coordinate;
    }

    public Coordinate Coordinate { get; }

    public int TotalDistance => DistanceFromStart + DistanceToEnd;

    public int DistanceFromStart { get; private set; } = -1;
    public int DistanceToEnd     { get; private set; } = -1;

    public Point? Origin { get; private set; }

    public bool Blocked { get; private set; }
    public bool Visited { get; private set; }

    public override int GetHashCode() => Convert.ToInt32($"{Coordinate.X}{Coordinate.Y}");

    public void SetBlocked(bool isBlocked) => Blocked = isBlocked;

    // Estimated distance ignoring blocked points
    public void SetDistanceToEnd(Coordinate endPointCoordinate) => DistanceToEnd = Math.Abs(endPointCoordinate.X - Coordinate.X) + Math.Abs(endPointCoordinate.Y - Coordinate.Y);

    public void SetDistanceFromStart(int distance) => DistanceFromStart = distance;

    public void SetOrigin(Point origin) => Origin = origin;

    public void Reset()
    {
        DistanceFromStart = -1;
        DistanceToEnd     = -1;
        Origin            = null;
        Visited           = false;
    }

    public bool Equals(Point? other) => other is { } otherPoint && otherPoint.Coordinate.Equals(Coordinate);
}