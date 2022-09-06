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

    private Point? _endPoint;
    private Point? _startPoint;

    public Grid(uint xSize, uint ySize)
    {
        Size = (xSize, ySize);

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++) { _grid.Add(new Point(x, y)); }
        }
    }

    public (uint X, uint Y) Size { get; }

    public IReadOnlyList<Point> Points => _grid.ToImmutableList();

    public void SetStartPoint(Point startPoint)
    {
        IsAllowedPoint(startPoint);
        if (startPoint.Equals(_endPoint)) throw new InvalidOperationException(@"StartPoint cannot be same as EndPoint");

        _startPoint = startPoint;
    }

    public void SetEndPoint(Point endPoint)
    {
        IsAllowedPoint(endPoint);
        if (endPoint.Equals(_startPoint)) throw new InvalidOperationException(@"EndPoint cannot be same as StartPoint");

        _endPoint = endPoint;
    }

    public IReadOnlySet<Point> GetShortestPath()
    {
        if (_startPoint == null || _endPoint == null) throw new InvalidOperationException("Cannot calculate path without start and end point.");

        return this.ShortestPath(_startPoint, _endPoint);
    }

    private void IsAllowedPoint(Point point)
    {
        if (_grid.SingleOrDefault(p => p.X == point.X && p.Y == point.Y) is not { } gridPoint) throw new InvalidOperationException(@"Point coordinate invalid.");
        if (gridPoint.Blocked) throw new InvalidOperationException(@"Point coordinate is blocked.");
    }
}