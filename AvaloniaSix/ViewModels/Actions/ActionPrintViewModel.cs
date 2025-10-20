using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AvaloniaSix.ViewModels;

public partial class ActionPrintViewModel : ViewModelBase
{
    [property: JsonIgnore]
    private string _savedState = "";

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
    private string _printerProfileId = "";

    [JsonIgnore]
    public bool HasChanged => IsNewItem || (_savedState != "" && _savedState != JsonSerializer.Serialize(this));

    public void SetSaveState()
    {
        _savedState = JsonSerializer.Serialize(this);

        OnPropertyChanged(nameof(HasChanged));
    }

    [RelayCommand]
    public void RestoreState()
    {
        var store = JsonSerializer.Deserialize<ActionPrintViewModel>(_savedState);

        var properties = GetType().GetProperties()
            .Where(x => x.CanWrite && x.GetCustomAttribute<JsonIgnoreAttribute>() == null);
        foreach (var item in properties)
        {
            var value = item.GetValue(store);
            item.SetValue(this, value);
        }
    }
}
