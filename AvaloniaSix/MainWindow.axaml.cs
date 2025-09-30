using Avalonia.Controls;

namespace AvaloniaSix;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Image_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        if (e.ClickCount != 2) return;

        var ctx = DataContext as ViewModels.MainViewModel;
        ctx?.SideMenuToggleCommand.Execute(null);
    }
}