using Avalonia.Svg.Skia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExCSS;

namespace AvaloniaSix.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    //[NotifyPropertyChangedFor(nameof(SideMenuImg))]
    private bool _sideMenuExpanded = true;

    //public SvgImage SideMenuImg => new() { Source = SvgSource.Load(@$"avares://{nameof(AvaloniaSix)}/Assets/Images/{(_sideMenuExpanded ? "flower_banner" : "icon")}.svg") };

    [RelayCommand]
    private void SideMenuToggle() => SideMenuExpanded = !SideMenuExpanded;
}
