// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Dijkstra.MapView.xaml.cs
// Created on: 20220907
// -----------------------------------------------

using System.Windows.Input;
using System.Windows.Media.Imaging;
using Dijkstra.Models;
using Dijkstra.ViewModels;

namespace Dijkstra.Views;

public partial class MapView
{
    private readonly GridViewModel   _gridViewModel;
    private readonly WriteableBitmap _image;
    private          bool            _drawingActive;

    public MapView(GridViewModel gridViewModel)
    {
        _gridViewModel = gridViewModel;
        _image         = (WriteableBitmap)_gridViewModel.BitmapVm.Source;

        InitializeComponent();

        DataContext = _gridViewModel;
    }

    private void OnMouseRightDown(object sender, MouseButtonEventArgs e)
    {
        _drawingActive = true;
        OnMouseMove(sender, e);
    }

    private void OnMouseRightUp(object sender, MouseButtonEventArgs e) => _drawingActive = false;
    private void OnMouseLeave(object   sender, MouseEventArgs       e) => _drawingActive = false;

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
        if (!_gridViewModel.CanCalculate || !_drawingActive) return;

        try { _gridViewModel.BlockCoordinate(CurrentCoordinate(_image, e)); }
        catch
        {
            // ignored
        }
    }

    private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (!_gridViewModel.CanCalculate) return;

        LeftAction action = _gridViewModel.StartLocation == null ? LeftAction.SetStart : _gridViewModel.EndLocation == null ? LeftAction.SetEnd : LeftAction.Reset;

        if (action == LeftAction.Reset)
        {
            _gridViewModel.ResetLocations();
            return;
        }

        Coordinate newLocation = CurrentCoordinate(_image, e);

        if (!ValidateCoordinate(action, newLocation))
        {
            _gridViewModel.ShowError("Cannot place here.");
            return;
        }

        switch (action)
        {
            case LeftAction.SetStart:
                _gridViewModel.SetStartLocation(newLocation);
                break;
            case LeftAction.SetEnd:
                _gridViewModel.SetEndLocation(newLocation);
                break;
        }
    }

    private bool ValidateCoordinate(LeftAction action, Coordinate coordinate)
    {
        if (_gridViewModel.PointAtCoordinate(coordinate).Blocked) return false;

        return action switch
        {
            LeftAction.SetStart => !coordinate.Equals(_gridViewModel.EndLocation),
            LeftAction.SetEnd   => !coordinate.Equals(_gridViewModel.StartLocation),
            _                   => false
        };
    }

    private Coordinate CurrentCoordinate(WriteableBitmap image, MouseEventArgs e)
    {
        int pixelMousePositionX = (int)(e.GetPosition(Map).X * image.PixelWidth  / Map.ActualWidth);
        int pixelMousePositionY = (int)(e.GetPosition(Map).Y * image.PixelHeight / Map.ActualHeight);

        return new Coordinate(pixelMousePositionX, pixelMousePositionY);
    }

    private enum LeftAction
    {
        SetStart,
        SetEnd,
        Reset
    }
}