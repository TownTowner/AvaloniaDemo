using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AvaloniaSix.ViewModels;

public partial class ActionPrintViewModel : ViewModelBase
{
    [JsonIgnore]
    private string _savedState = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _id = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _jobName = "";

    [ObservableProperty]
    private bool _isSelected;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _description = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _printDrawingRange = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _drawingExclusionList = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    [NotifyPropertyChangedFor(nameof(DrawingExclusionListTitle))]
    private bool _isDrawingExclusionIsWhiteList;

    public string DrawingExclusionListTitle => IsDrawingExclusionIsWhiteList ? "White List" : "Black List";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    private bool _isPrintModel;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    private bool _isPrintDrawing;

    [ObservableProperty]
    private bool _isNewItem;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    private ActionPrinterProfileViewModel _printerProfile = new();

    [JsonIgnore]
    public bool HasChanged => _savedState != JsonSerializer.Serialize(this);

    public void SetSaveState()
    {
        _savedState = JsonSerializer.Serialize(this);

        OnPropertyChanged(nameof(HasChanged));
    }
}
