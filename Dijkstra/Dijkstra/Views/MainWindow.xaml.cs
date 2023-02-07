// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Dijkstra.MainWindow.xaml.cs
// Created on: 20220908
// -----------------------------------------------

using System.Threading.Tasks;
using System.Windows;
using Dijkstra.ViewModels;

namespace Dijkstra.Views;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private bool _resetMessage;

    public MainWindow(GridViewModel gridViewModel, UIElement mapView)
    {
        InitializeComponent();

        MapViewLocation.Children.Add(mapView);

        DataContext = gridViewModel;

        gridViewModel.NewMessage += OnNewMessage;

        OnNewMessage("Use your left mouse button to place the start and end points, use your right mouse button to draw walls.", false);
    }

    private void OnNewMessage(string message, bool reset)
    {
        MessageBlock.Text = message;
        if (reset) ClearErrors();

        _resetMessage = reset;
    }

    private async void ClearErrors()
    {
        await Task.Delay(5000);
        if (_resetMessage) MessageBlock.Text = string.Empty;
    }
}