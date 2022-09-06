// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Dijkstra.tests.UnitTest1.cs
// Created on: 20220906
// -----------------------------------------------

using AutoFixture;
using FluentAssertions;

namespace Dijkstra.tests;

public class SomeClass
{
    public int Echo(int someInt) => someInt;
}

public class UnitTest1
{
    [Fact]
    public void ShortestPathSimpleSmallGrid()
    {
        // Arrange
        Fixture fixture = new();

        int expectedNumber = fixture.Create<int>();

        SomeClass sut = fixture.Create<SomeClass>();

        // Act
        int result = sut.Echo(expectedNumber);

        // Assert
        result.Should().Be(expectedNumber);
    }
}