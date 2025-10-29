using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace AvaloniaSix.ViewModels;

public partial class ActionPrintSettingsViewModel : ConfirmDialogViewModel
{
    [ObservableProperty]
    private string _id = string.Empty;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty] 
    private bool _canEdit = true;

    [ObservableProperty] 
    private bool _canDelete = true;

    [ObservableProperty]
    private int _copies = 0;

    [ObservableProperty]
    private ObservableCollection<ActionPrintSettingsProfileViewModel> _printSettingsProfiles = [];

    public ActionPrintSettingsViewModel()
    {
        Title = "Printer Settings";
        Message = "Configure the printer settings below.";
        ConfirmText = "Save";
        CancelText = "Cancel";

        InitializeDesignData();
    }

    protected override void OnDesignConstructor()
    {
        base.OnDesignConstructor();

        InitializeDesignData();
    }

    private void InitializeDesignData()
    {
        var profileSetting = new ActionPrintSettingsProfileViewModel()
        {
            Id = "0",
            Width = 210,
            Height = 297,
            Orientation = "Portrait",
            PaperSize = "A4",
        };
        PrintSettingsProfiles = [profileSetting, profileSetting, profileSetting];
    }
}
