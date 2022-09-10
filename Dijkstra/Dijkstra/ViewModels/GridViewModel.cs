// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Dijkstra.GridViewModel.cs
// Created on: 20220907
// -----------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Dijkstra.Models;
using Dijkstra.Resources;
using Prism.Commands;
using Point = Dijkstra.Models.Point;

namespace Dijkstra.ViewModels;

public class GridViewModel : ViewModelBase
{
    private readonly Grid _gridModel;

    private bool _aStarEnabled = true;
    private bool _canCalculate = true;
    private bool _canClear     = true;

    private string _errorMessage = "Use your left mouse button to place the start and end points, use your right mouse button to draw walls.";

    public GridViewModel(Grid gridModel, GridBitmap gridBitmap)
    {
        _gridModel = gridModel;
        BitmapVm   = gridBitmap;

        CalculateRouteCommand = new DelegateCommand(CalculateShortestPath, () => CanCalculate);
        ToggleAStarCommand    = new DelegateCommand(ToggleAStar,           () => CanCalculate);
        ClearCommand          = new DelegateCommand(ClearGrid,             () => CanClear);

        _gridModel.PointVisited += OnPointVisited;
    }

    public DelegateCommand CalculateRouteCommand { get; }

    public DelegateCommand ClearCommand { get; }

    public DelegateCommand ToggleAStarCommand { get; }

    public string AStarButtonText => _aStarEnabled ? "Turn A* off" : "Turn A* on";

    public Coordinate? StartLocation => _gridModel.StartPoint?.Coordinate;

    public Coordinate? EndLocation => _gridModel.EndPoint?.Coordinate;

    public GridBitmap BitmapVm { get; }

    public double Width => BitmapVm.Width * 10;

    public double Height => BitmapVm.Height * 10;

    public bool CanCalculate
    {
        get => _canCalculate;
        private set
        {
            if (!Set(ref _canCalculate, value)) return;

            CalculateRouteCommand.RaiseCanExecuteChanged();
            ToggleAStarCommand.RaiseCanExecuteChanged();
        }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        private set => Set(ref _errorMessage, value);
    }

    private bool CanClear
    {
        get => _canClear;
        set
        {
            if (!Set(ref _canClear, value)) return;

            ClearCommand.RaiseCanExecuteChanged();
        }
    }

    public void ShowError(string errorMessage) => ErrorMessage = errorMessage;

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
        _gridModel.PointAtCoordinate(coordinate).SetBlocked(true);

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

    public Point PointAtCoordinate(Coordinate coordinate) => _gridModel.PointAtCoordinate(coordinate);

    private void ToggleAStar()
    {
        _aStarEnabled = !_aStarEnabled;
        RaisePropertyChanged(nameof(AStarButtonText));
    }

    private void OnPointVisited(Coordinate coordinate) => Application.Current.Dispatcher.BeginInvoke(new Action(() => BitmapVm.WritePixel(coordinate, GridColors.VisitColor)));

    private void ClearGrid()
    {
        _gridModel.Clear();
        BitmapVm.Reset();

        CanCalculate = true;
    }

    private void ResetGrid()
    {
        _gridModel.Reset(true);
        BitmapVm.Reset();

        if (_gridModel.StartPoint != null) BitmapVm.WritePixel(_gridModel.StartPoint.Coordinate, GridColors.StartColor);
        if (_gridModel.EndPoint   != null) BitmapVm.WritePixel(_gridModel.EndPoint.Coordinate,   GridColors.EndColor);
    }

    private void CalculateShortestPath()
    {
        if (_gridModel.StartPoint == null || _gridModel.EndPoint == null)
        {
            ErrorMessage = "No start or endpoint set.";
            return;
        }

        CanCalculate = false;
        CanClear     = false;

        ResetGrid();

        Task<IReadOnlySet<Point>> pathTask = Task.Run(() => _gridModel.GetShortestPath(_aStarEnabled));

        pathTask.ContinueWith(_ =>
        {
            ShowError("Cannot calculate route.");
            CanClear = true;
        }, TaskContinuationOptions.OnlyOnFaulted);

        pathTask.ContinueWith(result =>
                              {
                                  result.Result.Skip(1)
                                        .SkipLast(1)
                                        .ToList()
                                        .ForEach(point => Application.Current.Dispatcher.BeginInvoke(new Action(() => BitmapVm.WritePixel(point.Coordinate, GridColors.RouteColor))));
                                  CanClear     = true;
                                  CanCalculate = true;
                              },
                              TaskContinuationOptions.OnlyOnRanToCompletion);
    }
}