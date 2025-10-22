using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace AvaloniaSix.ViewModels;

public partial class ActionPrinterProfileViewModel : ConfirmDialogViewModel
{
    [ObservableProperty]
    private string _id = string.Empty;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private int _copies = 0;

    private ObservableCollection<ActionPrinterSettingsViewModel> _printerSettings = [];

    public ActionPrinterProfileViewModel()
    {
        Title = "Printer Profile";
        Message = "Configure the printer profile settings below.";
        ConfirmText = "Save";
        CancelText = "Cancel";
    }
}
