using Avalonia.Svg.Skia;
using AvaloniaSix.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExCSS;

namespace AvaloniaSix.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    //[NotifyPropertyChangedFor(nameof(SideMenuImg))]
    private bool _sideMenuExpanded = true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HomePageIsActive))]
    [NotifyPropertyChangedFor(nameof(ProcessPageIsActive))]
    private ViewModelBase _currentPage;

    public bool HomePageIsActive => CurrentPage == _homePageVM;
    public bool ProcessPageIsActive => CurrentPage == _processPageVM;

    private HomePageViewModel _homePageVM = new();
    private ProcessPageViewModel _processPageVM = new();

    public MainViewModel()
    {
        CurrentPage = _homePageVM;
    }

    //public SvgImage SideMenuImg => new() { Source = SvgSource.Load(@$"avares://{nameof(AvaloniaSix)}/Assets/Images/{(_sideMenuExpanded ? "flower_banner" : "icon")}.svg") };

    [RelayCommand]
    private void SideMenuToggle() => SideMenuExpanded = !SideMenuExpanded;

    [RelayCommand]
    private void GoToHome() => CurrentPage = _homePageVM;

    [RelayCommand]
    private void GoToProcess() => CurrentPage = _processPageVM;
}
