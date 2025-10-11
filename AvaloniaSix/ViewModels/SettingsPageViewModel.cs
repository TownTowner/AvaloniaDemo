using AvaloniaSix.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace AvaloniaSix.ViewModels;

public partial class SettingsPageViewModel : PageViewModel
{
    [ObservableProperty]
    private List<string> _locations;

    public SettingsPageViewModel()
    {
        PageName = ApplicationPageName.Settings;
        _locations = new List<string>
        {
            "C:\\Program Files\\REDACTED_PROJECT_NAME",
            "D:\\Games\\REDACTED_PROJECT_NAME",
            "E:\\Applications\\REDACTED_PROJECT_NAME"
        };
    }
}
