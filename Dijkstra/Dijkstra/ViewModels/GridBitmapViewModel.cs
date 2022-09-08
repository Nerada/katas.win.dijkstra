// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Dijkstra.GridBitmapViewModel.cs
// Created on: 20220907
// -----------------------------------------------

using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Dijkstra.Models;
using Dijkstra.Resources;

namespace Dijkstra.ViewModels;

public class GridBitmapViewModel
{
    private const    int             Stride     = 4;
    private readonly byte[]          _colorData = new byte[Stride];
    private readonly WriteableBitmap _writeableBitmap;

    public GridBitmapViewModel(Grid grid)
    {
        _writeableBitmap = new WriteableBitmap((int)grid.Size.X, (int)grid.Size.Y, 96.0, 96.0, PixelFormats.Pbgra32, null);

        grid.Points.ToList().ForEach(point => WritePixel(point.Coordinate, point.Blocked ? GridColors.WallColor : GridColors.BackgroundColor));
    }

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
}