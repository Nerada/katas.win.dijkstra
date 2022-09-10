// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Dijkstra.GridColors.cs
// Created on: 20220907
// -----------------------------------------------

using System.Windows.Media;

namespace Dijkstra.Resources;

public static class GridColors
{
    public static readonly Color BackgroundColor = Color.FromRgb(0xee, 0xee, 0xee); //#eeeeee
    public static readonly Color EndColor        = Color.FromRgb(0xff, 0x4f, 0x00); //#ff4f00
    public static readonly Color RouteColor      = Color.FromRgb(0xf8, 0xe3, 0x67); //#f8e367
    public static readonly Color StartColor      = Color.FromRgb(0x00, 0xff, 0x4f); //#00FF4F
    public static readonly Color VisitColor      = Color.FromRgb(0xdd, 0xdd, 0xdd); //#dddddd
    public static readonly Color WallColor       = Color.FromRgb(0x33, 0x33, 0x33); //#333333
}