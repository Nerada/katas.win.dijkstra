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
        grid.SetEndPoint(new Point(4,   4));

        // Act
        IReadOnlySet<Point> shortestPath = grid.GetShortestPath();

        // Assert
        shortestPath.Should().HaveCount(4);
        shortestPath.Should().ContainSingle(p => p.Equals(new Point(1, 1)));
        shortestPath.Should().ContainSingle(p => p.Equals(new Point(2, 2)));
        shortestPath.Should().ContainSingle(p => p.Equals(new Point(3, 3)));
        shortestPath.Should().ContainSingle(p => p.Equals(new Point(4, 4)));
    }
}