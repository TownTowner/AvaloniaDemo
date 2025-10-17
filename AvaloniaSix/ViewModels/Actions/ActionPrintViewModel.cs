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

    [ObservableProperty]
    private string _description;

    [ObservableProperty]
    private string _printDrawingRange;

    [ObservableProperty]
    private string _drawingExclusionList;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DrawingExlusionListTitle))]
    private bool _isDrawingExlusionIsWhiteList;

    public string DrawingExlusionListTitle => IsDrawingExlusionIsWhiteList ? "White List" : "Black List";

    [ObservableProperty]
    private bool _isPrintModel;

    [ObservableProperty]
    private bool _isPrintDrawing;

    [ObservableProperty]
    private bool _isNewItem;

}
