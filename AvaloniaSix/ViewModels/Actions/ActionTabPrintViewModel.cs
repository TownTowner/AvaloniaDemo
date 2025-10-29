using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;

namespace AvaloniaSix.ViewModels;

public partial class ActionTabPrintViewModel : ViewModelBase
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _id = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _jobName = "";

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
    private bool _drawingExclusionIsWhiteList;

    public string DrawingExclusionListTitle => DrawingExclusionIsWhiteList ? "White List" : "Black List";

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
    private string _printSettingsId = "";

    [JsonIgnore]
    public override bool HasChanged => IsNewItem || base.HasChanged;

}
