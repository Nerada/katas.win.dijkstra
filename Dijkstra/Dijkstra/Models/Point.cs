// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Dijkstra.Point.cs
// Created on: 20220909
// -----------------------------------------------

using System;

namespace Dijkstra.Models;

public readonly record struct Coordinate(int X, int Y);

public delegate void VisitEventHandler(Coordinate coordinate);

public class Point
{
    public Point(Coordinate coordinate)
    {
        Coordinate = coordinate;
    }

    public Coordinate Coordinate { get; }

    public int TotalDistance => DistanceFromStart + DistanceToEnd;

    public Point? Origin { get; private set; }

    public int DistanceFromStart { get; private set; } = -1;
    public int DistanceToEnd     { get; private set; } = -1;

    public bool Blocked { get; private set; }

    public event VisitEventHandler? PointVisited;

    public override int GetHashCode() => Convert.ToInt32($"{Coordinate.X}{Coordinate.Y}");

    public void SetBlocked(bool isBlocked) => Blocked = isBlocked;

    // Estimated distance ignoring blocked points
    public void SetDistanceToEnd(Coordinate endPointCoordinate) => DistanceToEnd = Math.Abs(endPointCoordinate.X - Coordinate.X) + Math.Abs(endPointCoordinate.Y - Coordinate.Y);

    public void Visit() => PointVisited?.Invoke(Coordinate);

    public void Reset(bool soft = false)
    {
        DistanceFromStart = -1;
        DistanceToEnd     = -1;
        Origin            = null;
        if (!soft) Blocked = false;
    }

    public override bool Equals(object? obj) => obj is Point otherPoint && otherPoint.Coordinate.Equals(Coordinate);

    public void SetOrigin(Point origin)
    {
        Origin            = origin;
        DistanceFromStart = origin.DistanceFromStart + 1;
    }
}