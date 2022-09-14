// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Dijkstra.Point.cs
// Created on: 20220913
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

    public int TotalDistance     => DistanceFromStart + DistanceToEnd;
    public int DistanceFromStart => Origin == null ? -1 : Origin.DistanceFromStart + 1;

    public bool Blocked { get; private set; }

    public Point? Origin { get; private set; }

    private int DistanceToEnd { get; set; } = -1;

    public event VisitEventHandler? Visited;

    public void Visit() => Visited?.Invoke(Coordinate);

    public void SetBlocked(bool isBlocked) => Blocked = isBlocked;

    public void SetOrigin(Point origin) => Origin = origin;

    /// <summary>
    ///     Calculates distance to end-point ignoring walls
    /// </summary>
    public void SetDistanceToEnd(Coordinate endPointCoordinate) => DistanceToEnd = Math.Abs(endPointCoordinate.X - Coordinate.X) + Math.Abs(endPointCoordinate.Y - Coordinate.Y);

    /// <summary>
    ///     Soft reset will keep existing walls (blocked points)
    /// </summary>
    public void Reset(bool soft = false)
    {
        DistanceToEnd = -1;
        Origin        = null;
        if (!soft) Blocked = false;
    }

    public override bool Equals(object? obj) => obj is Point otherPoint && otherPoint.Coordinate.Equals(Coordinate);

    public override int GetHashCode() => Convert.ToInt32($"{Coordinate.X}{Coordinate.Y}");
}