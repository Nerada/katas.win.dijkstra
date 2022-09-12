// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Dijkstra.MainWindow.xaml.cs
// Created on: 20220908
// -----------------------------------------------

using System.ComponentModel;
using System.Threading.Tasks;
using Dijkstra.ViewModels;

namespace Dijkstra.Views;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private readonly GridViewModel _gridViewModel;

    public MainWindow(GridViewModel gridViewModel, MapView mapView)
    {
        _gridViewModel = gridViewModel;

        InitializeComponent();

        MapViewLocation.Children.Add(mapView);

        DataContext = gridViewModel;

        _gridViewModel.PropertyChanged += OnGameViewModelPropertyChanged;
    }

    private void OnGameViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(GridViewModel.Message) && _gridViewModel.ResetMessage) ClearErrors();
    }

    private async void ClearErrors()
    {
        await Task.Delay(5000);
        _gridViewModel.ShowMessage(string.Empty);
    }
}