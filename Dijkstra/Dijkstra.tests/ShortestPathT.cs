// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Dijkstra.tests.ShortestPathT.cs
// Created on: 20220906
// -----------------------------------------------

using Dijkstra.Models;
using FluentAssertions;

namespace Dijkstra.tests;

public class ShortestPathT
{
    [Fact]
    public void ShortestPathSimpleSmallGrid()
    {
        // Arrange
        Grid grid = new(5, 5);
        grid.SetStartPoint(new Coordinate(1, 1));
        grid.SetEndPoint(new Coordinate(3,   3));

        // Act
        IReadOnlySet<Point> shortestPath = grid.GetShortestPath();

        // Assert
        shortestPath.Should().HaveCount(5);
        shortestPath.Should().ContainSingle(p => p.Equals(new Point(new Coordinate(1, 1))));
        shortestPath.Should().ContainSingle(p => p.Equals(new Point(new Coordinate(3, 3))));
    }

    [Fact]
    public void DirectRouteBlocked()
    {
        // Arrange
        Grid grid = new(5, 5);
        grid.SetStartPoint(new Coordinate(1, 1));
        grid.SetEndPoint(new Coordinate(3,   3));

        grid.Points.Single(p => p.Coordinate.X == 1 && p.Coordinate.Y == 2).SetBlocked(true);
        grid.Points.Single(p => p.Coordinate.X == 2 && p.Coordinate.Y == 1).SetBlocked(true);

        // Act
        IReadOnlySet<Point> shortestPath = grid.GetShortestPath();

        // Assert
        shortestPath.Should().HaveCount(7);
        shortestPath.Should().ContainSingle(p => p.Equals(new Point(new Coordinate(1, 1))));
        shortestPath.Should().ContainSingle(p => p.Equals(new Point(new Coordinate(3, 3))));
    }

    [Fact]
    public void TotalCutoff()
    {
        // Arrange
        Grid grid = new(5, 5);
        grid.SetStartPoint(new Coordinate(1, 1));
        grid.SetEndPoint(new Coordinate(3,   3));

        grid.Points.Single(p => p.Coordinate.X == 0 && p.Coordinate.Y == 2).SetBlocked(true);
        grid.Points.Single(p => p.Coordinate.X == 1 && p.Coordinate.Y == 2).SetBlocked(true);
        grid.Points.Single(p => p.Coordinate.X == 2 && p.Coordinate.Y == 2).SetBlocked(true);
        grid.Points.Single(p => p.Coordinate.X == 3 && p.Coordinate.Y == 2).SetBlocked(true);
        grid.Points.Single(p => p.Coordinate.X == 4 && p.Coordinate.Y == 2).SetBlocked(true);

        // Act/Assert
        grid.Invoking(g => g.GetShortestPath()).Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void NoStartSet()
    {
        // Arrange
        Grid grid = new(5, 5);
        grid.SetEndPoint(new Coordinate(3, 3));

        // Act/Assert
        grid.Invoking(g => g.GetShortestPath()).Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void PointOutsideGrid()
    {
        // Arrange
        Grid grid = new(5, 5);

        // Act/Assert
        grid.Invoking(g => g.SetStartPoint(new Coordinate(10, 3))).Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void SameStartAndEndPoint()
    {
        // Arrange
        Grid grid = new(5, 5);

        grid.SetStartPoint(new Coordinate(2, 2));

        // Act/Assert
        grid.Invoking(g => g.SetEndPoint(new Coordinate(2, 2))).Should().Throw<InvalidOperationException>();
    }
}