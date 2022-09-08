// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Dijkstra.App.xaml.cs
// Created on: 20220906
// -----------------------------------------------

using System.Windows;
using Dijkstra.Models;
using Dijkstra.ViewModels;
using Dijkstra.Views;

namespace Dijkstra;

/// <summary>
///     https://en.wikipedia.org/wiki/A*_search_algorithm
/// </summary>
public partial class App
{
    private void OnStartup(object sender, StartupEventArgs e)
    {
        Grid grid = new(100, 60);

        GridBitmapViewModel bitmap = new(grid);

        GridViewModel gridViewModel = new(grid, bitmap);

        MainWindow mainWindow = new()
        {
            DataContext = gridViewModel
        };

        mainWindow.Show();
    }
}