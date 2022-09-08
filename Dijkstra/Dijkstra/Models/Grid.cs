// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Dijkstra.Grid.cs
// Created on: 20220906
// -----------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Dijkstra.Models;

public class Grid
{
    private readonly HashSet<Point> _grid = new();

    public Grid(uint xSize, uint ySize)
    {
        Size = (xSize, ySize);

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++) { _grid.Add(new Point(new Coordinate(x, y))); }
        }
    }

    public (uint X, uint Y) Size { get; }

    public IReadOnlyList<Point> Points => _grid.ToImmutableList();

    public Point? EndPoint   { get; private set; }
    public Point? StartPoint { get; private set; }

    public void SetStartPoint(Coordinate? startPoint)
    {
        if (startPoint is not { } start)
        {
            StartPoint = null;
            return;
        }

        if (start.Equals(EndPoint?.Coordinate)) throw new InvalidOperationException(@"StartPoint cannot be same as EndPoint");

        StartPoint = GetRoutePoint(start);
    }

    public void SetEndPoint(Coordinate? endPoint)
    {
        if (endPoint is not { } end)
        {
            EndPoint = null;
            return;
        }

        if (end.Equals(StartPoint?.Coordinate)) throw new InvalidOperationException(@"EndPoint cannot be same as StartPoint");

        EndPoint = GetRoutePoint(end);
    }

    public IReadOnlySet<Point> GetShortestPath()
    {
        if (StartPoint == null || EndPoint == null) throw new InvalidOperationException("Cannot calculate path without start and end point.");

        return this.ShortestPath(StartPoint, EndPoint);
    }

    private Point GetRoutePoint(Coordinate coordinate)
    {
        if (_grid.SingleOrDefault(p => p.Coordinate.Equals(coordinate)) is not { } gridPoint) throw new InvalidOperationException(@"Point coordinate invalid.");
        if (gridPoint.Blocked) throw new InvalidOperationException(@"Point coordinate is blocked.");

        return gridPoint;
    }
}