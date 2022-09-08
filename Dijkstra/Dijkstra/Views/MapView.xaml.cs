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
    private bool _drawingActive;

    private GridViewModel?   _gridViewModel;
    private WriteableBitmap? _image;

    public MapView()
    {
        InitializeComponent();


        DataContextChanged += (_, _) => DataContextSet();
    }

    private void DataContextSet()
    {
        _gridViewModel = (GridViewModel)DataContext;

        _image = (WriteableBitmap)_gridViewModel.BitmapVm.Source;
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
        if (_image == null || _gridViewModel == null || !_drawingActive) return;

        _gridViewModel.BlockCoordinate(CurrentCoordinate(_image, e));
    }

    private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (_image == null || _gridViewModel == null) return;

        if (_gridViewModel.StartLocation == null)
        {
            _gridViewModel.SetStartLocation(CurrentCoordinate(_image, e));
            return;
        }

        if (_gridViewModel.EndLocation == null)
        {
            _gridViewModel.SetEndLocation(CurrentCoordinate(_image, e));
            return;
        }

        _gridViewModel.ResetLocations();
    }

    private Coordinate CurrentCoordinate(WriteableBitmap image, MouseEventArgs e)
    {
        int pixelMousePositionX = (int)(e.GetPosition(Map).X * image.PixelWidth  / Map.ActualWidth);
        int pixelMousePositionY = (int)(e.GetPosition(Map).Y * image.PixelHeight / Map.ActualHeight);

        return new Coordinate(pixelMousePositionX, pixelMousePositionY);
    }
}