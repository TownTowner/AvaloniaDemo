using AvaloniaSix.Data;
using AvaloniaSix.Factories;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaSix.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private PageFactory _pageFactory;

    [ObservableProperty]
    //[NotifyPropertyChangedFor(nameof(SideMenuImg))]
    private bool _sideMenuExpanded = true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsActionsPage))]
    [NotifyPropertyChangedFor(nameof(IsHistoryPage))]
    [NotifyPropertyChangedFor(nameof(IsHomePage))]
    [NotifyPropertyChangedFor(nameof(IsMacrosPage))]
    [NotifyPropertyChangedFor(nameof(IsProcessPage))]
    [NotifyPropertyChangedFor(nameof(IsReporterPage))]
    [NotifyPropertyChangedFor(nameof(IsSettingsPage))]
    private PageViewModel _currentPage;

    public bool IsActionsPage => CurrentPage.PageName == ApplicationPageName.Actions;
    public bool IsHistoryPage => CurrentPage.PageName == ApplicationPageName.History;
    public bool IsHomePage => CurrentPage.PageName == ApplicationPageName.Home;
    public bool IsMacrosPage => CurrentPage.PageName == ApplicationPageName.Macros;
    public bool IsProcessPage => CurrentPage.PageName == ApplicationPageName.Process;
    public bool IsReporterPage => CurrentPage.PageName == ApplicationPageName.Reporter;
    public bool IsSettingsPage => CurrentPage.PageName == ApplicationPageName.Settings;


    public MainViewModel(PageFactory pageFactory)
    {
        this._pageFactory = pageFactory;
        GoToHome();
    }

    //public SvgImage SideMenuImg => new() { Source = SvgSource.Load(@$"avares://{nameof(AvaloniaSix)}/Assets/Images/{(_sideMenuExpanded ? "flower_banner" : "icon")}.svg") };

    [RelayCommand]
    private void SideMenuToggle() => SideMenuExpanded = !SideMenuExpanded;

    [RelayCommand]
    private void GoToActions() => CurrentPage = _pageFactory.CreatePage(ApplicationPageName.Actions);

    [RelayCommand]
    private void GoToHistory() => CurrentPage = _pageFactory.CreatePage(ApplicationPageName.History);

    [RelayCommand]
    private void GoToHome() => CurrentPage = _pageFactory.CreatePage(ApplicationPageName.Home);

    [RelayCommand]
    private void GoToMacros() => CurrentPage = _pageFactory.CreatePage(ApplicationPageName.Macros);

    [RelayCommand]
    private void GoToProcess() => CurrentPage = _pageFactory.CreatePage(ApplicationPageName.Process);

    [RelayCommand]
    private void GoToReporter() => CurrentPage = _pageFactory.CreatePage(ApplicationPageName.Reporter);

    [RelayCommand]
    private void GoToSettings() => CurrentPage = _pageFactory.CreatePage(ApplicationPageName.Settings);
}
