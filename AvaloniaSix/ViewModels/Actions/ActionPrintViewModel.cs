using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace AvaloniaSix.ViewModels;

public partial class ActionPrintViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _id;

    [ObservableProperty]
    private string _jobName;

    [ObservableProperty]
    private bool _isSelected;
}
