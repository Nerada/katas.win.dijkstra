// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Dijkstra.Grid.cs
// Created on: 20220909
// -----------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Dijkstra.Extensions;

namespace Dijkstra.Models;

public class Grid
{
    private readonly HashSet<Point> _grid = new();

    public Grid(uint xSize, uint ySize)
    {
        Size = (xSize, ySize);

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                Point newPoint = new(new Coordinate(x, y));

                _grid.Add(newPoint);

                newPoint.Visited += OnPointVisited;
            }
        }
    }

    public (uint X, uint Y) Size { get; }

    public IReadOnlyList<Point> Points => _grid.ToImmutableList();

    public Point? EndPoint   { get; private set; }
    public Point? StartPoint { get; private set; }

    public event VisitEventHandler? PointVisited;

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

    public IReadOnlySet<Point> GetShortestPath(bool aStarEnabled = true)
    {
        if (StartPoint == null || EndPoint == null) throw new InvalidOperationException("Cannot calculate path without start and end point.");

        return this.ShortestPath(StartPoint, EndPoint, aStarEnabled);
    }

    public void Reset(bool soft = false) => _grid.ToList().ForEach(point => point.Reset(soft));

    public Point PointAtCoordinate(Coordinate coordinate) => Points.Single(point => point.Coordinate.Equals(coordinate));

    public void Clear()
    {
        StartPoint = null;
        EndPoint   = null;

        Reset();
    }

    private void OnPointVisited(Coordinate coordinate)
    {
        if (coordinate.Equals(StartPoint?.Coordinate) || coordinate.Equals(EndPoint?.Coordinate)) return;

        PointVisited?.Invoke(coordinate);
    }

    private Point GetRoutePoint(Coordinate coordinate)
    {
        if (_grid.SingleOrDefault(p => p.Coordinate.Equals(coordinate)) is not { } gridPoint) throw new InvalidOperationException(@"Point coordinate invalid.");
        if (gridPoint.Blocked) throw new InvalidOperationException(@"Point coordinate is blocked.");

        return gridPoint;
    }
}