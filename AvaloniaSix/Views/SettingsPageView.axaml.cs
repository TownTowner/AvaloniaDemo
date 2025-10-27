using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaSix.ViewModels;

namespace AvaloniaSix.Views;

public partial class SettingsPageView : UserControl
{
    public SettingsPageView()
    {
        InitializeComponent();
    }

    private void OnViewLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        (DataContext as ViewModelBase)?.OnViewLoaded();
    }
}