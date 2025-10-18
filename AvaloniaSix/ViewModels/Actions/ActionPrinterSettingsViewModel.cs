using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaSix.ViewModels;

public partial class ActionPrinterSettingsViewModel : ViewModelBase
{
    [ObservableProperty]
    private string type;

    [ObservableProperty]
    private string _printerName;

    [ObservableProperty]
    private string _printerSize;

    [ObservableProperty]
    private double _width;

    [ObservableProperty]
    private double _height;

    [ObservableProperty]
    private bool? _portrait;

    [ObservableProperty]
    private string _sourceTray;

    [ObservableProperty]
    private string _drawingcolor;

    [ObservableProperty]
    private bool _scaleToFit;
}
