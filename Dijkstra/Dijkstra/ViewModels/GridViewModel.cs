// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Dijkstra.GridViewModel.cs
// Created on: 20220907
// -----------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Dijkstra.Models;
using Dijkstra.Resources;
using Prism.Commands;

namespace Dijkstra.ViewModels;

public class GridViewModel
{
    private readonly Grid _gridModel;

    public GridViewModel(Grid gridModel, GridBitmapViewModel gridBitmapViewModel)
    {
        _gridModel = gridModel;
        BitmapVm   = gridBitmapViewModel;

        CalculateRouteCommand = new DelegateCommand(CalculateShortestPath);
    }

    public DelegateCommand CalculateRouteCommand { get; }

    public Coordinate? StartLocation => _gridModel.StartPoint?.Coordinate;

    public Coordinate? EndLocation => _gridModel.EndPoint?.Coordinate;

    public GridBitmapViewModel BitmapVm { get; }

    public void SetStartLocation(Coordinate coordinate)
    {
        _gridModel.SetStartPoint(coordinate);

        BitmapVm.WritePixel(coordinate, GridColors.StartColor);
    }

    public void SetEndLocation(Coordinate coordinate)
    {
        _gridModel.SetEndPoint(coordinate);

        BitmapVm.WritePixel(coordinate, GridColors.EndColor);
    }

    public void BlockCoordinate(Coordinate coordinate)
    {
        _gridModel.Points.Single(point => point.Coordinate.Equals(coordinate)).SetBlocked(true);

        BitmapVm.WritePixel(coordinate, GridColors.WallColor);
    }

    public void ResetLocations()
    {
        if (_gridModel.StartPoint is { } oldStartPoint)
        {
            BitmapVm.WritePixel(oldStartPoint.Coordinate, oldStartPoint.Blocked ? GridColors.WallColor : GridColors.BackgroundColor);
            _gridModel.SetStartPoint(null);
        }

        if (_gridModel.EndPoint is { } oldEndPoint)
        {
            BitmapVm.WritePixel(oldEndPoint.Coordinate, oldEndPoint.Blocked ? GridColors.WallColor : GridColors.BackgroundColor);
            _gridModel.SetEndPoint(null);
        }
    }

    private void CalculateShortestPath()
    {
        IReadOnlySet<Point> path = _gridModel.GetShortestPath();

        path.Skip(1).SkipLast(1).ToList().ForEach(point => BitmapVm.WritePixel(point.Coordinate, GridColors.RouteColor));
    }
}