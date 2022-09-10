// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Dijkstra.GridBitmap.cs
// Created on: 20220907
// -----------------------------------------------

using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Dijkstra.Models;
using Dijkstra.Resources;

namespace Dijkstra.ViewModels;

public class GridBitmap
{
    private const    int    Stride     = 4;
    private readonly byte[] _colorData = new byte[Stride];
    private readonly Grid   _grid;

    private readonly int             _dpi = GetDpi();
    private readonly WriteableBitmap _writeableBitmap;

    public GridBitmap(Grid grid)
    {
        _grid            = grid;
        _writeableBitmap = new WriteableBitmap((int)grid.Size.X, (int)grid.Size.Y, _dpi, _dpi, PixelFormats.Pbgra32, null);

        Reset();
    }

    public double Width => _writeableBitmap.Width * _dpi / 100;

    public double Height => _writeableBitmap.Height * _dpi / 100;

    public ImageSource Source => _writeableBitmap;

    public void WritePixel(Coordinate location, Color color)
    {
        _colorData[0] = color.B;
        _colorData[1] = color.G;
        _colorData[2] = color.R;
        _colorData[3] = color.A;

        Int32Rect sourceRect = new(location.X, location.Y, 1, 1);
        _writeableBitmap.WritePixels(sourceRect, _colorData, Stride, 0);
    }

    public void Reset() => _grid.Points.ToList().ForEach(point => WritePixel(point.Coordinate, point.Blocked ? GridColors.WallColor : GridColors.BackgroundColor));

    private static int GetDpi()
    {
        PropertyInfo? dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);
        PropertyInfo? dpiYProperty = typeof(SystemParameters).GetProperty("Dpi",  BindingFlags.NonPublic | BindingFlags.Static);

        int dpiX = (int)(dpiXProperty?.GetValue(null, null) ?? throw new InvalidOperationException());
        int dpiY = (int)(dpiYProperty?.GetValue(null, null) ?? throw new InvalidOperationException());

        if (dpiX != dpiY) throw new InvalidOperationException("Dpi X and Y are different");

        return dpiY;
    }
}