using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace AvaloniaSix.ViewModels;

public partial class ActionPrinterProfileViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _description;

    [ObservableProperty]
    private int _copies;

    private ObservableCollection<ActionPrinterSettingsViewModel> _printerSettings;
}
