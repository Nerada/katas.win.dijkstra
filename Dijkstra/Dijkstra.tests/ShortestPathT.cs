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
    //Fixture fixture = new();
    //List<Point> grid = fixture.Create<List<Point>>();

    [Fact]
    public void ShortestPathSimpleSmallGrid()
    {
        // Arrange
        Grid grid = new(5, 5);
        grid.SetStartPoint(new Point(1, 1));
        grid.SetEndPoint(new Point(3,   3));

        // Act
        IReadOnlySet<Point> shortestPath = grid.GetShortestPath();

        // Assert
        shortestPath.Should().HaveCount(5);
        shortestPath.Should().ContainSingle(p => p.Equals(new Point(1, 1)));
        shortestPath.Should().ContainSingle(p => p.Equals(new Point(3, 3)));
    }

    [Fact]
    public void DirectRouteBlocked()
    {
        // Arrange
        Grid grid = new(5, 5);
        grid.SetStartPoint(new Point(1, 1));
        grid.SetEndPoint(new Point(3,   3));

        grid.Points.Single(p => p.X == 1 && p.Y == 2).SetBlocked(true);
        grid.Points.Single(p => p.X == 2 && p.Y == 1).SetBlocked(true);

        // Act
        IReadOnlySet<Point> shortestPath = grid.GetShortestPath();

        // Assert
        shortestPath.Should().HaveCount(7);
        shortestPath.Should().ContainSingle(p => p.Equals(new Point(1, 1)));
        shortestPath.Should().ContainSingle(p => p.Equals(new Point(3, 3)));
    }

    [Fact]
    public void TotalCutoff()
    {
        // Arrange
        Grid grid = new(5, 5);
        grid.SetStartPoint(new Point(1, 1));
        grid.SetEndPoint(new Point(3,   3));

        grid.Points.Single(p => p.X == 0 && p.Y == 2).SetBlocked(true);
        grid.Points.Single(p => p.X == 1 && p.Y == 2).SetBlocked(true);
        grid.Points.Single(p => p.X == 2 && p.Y == 2).SetBlocked(true);
        grid.Points.Single(p => p.X == 3 && p.Y == 2).SetBlocked(true);
        grid.Points.Single(p => p.X == 4 && p.Y == 2).SetBlocked(true);

        // Act/Assert
        grid.Invoking(g => g.GetShortestPath()).Should().Throw<InvalidOperationException>();
    }
}