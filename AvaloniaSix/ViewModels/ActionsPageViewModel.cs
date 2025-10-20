using AvaloniaSix.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AvaloniaSix.ViewModels;

public partial class ActionsPageViewModel() : PageViewModel(ApplicationPageName.Actions)
{
    private ActionPrinterProfileViewModel defaultProfile = new() { Copies = 1, Name = "(Default)PDF Printer", Description = @"Virtual Printers\Microsoft PDF" };

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PrintListHasItems))]
    private ObservableCollection<ActionPrintViewModel> _printList = [];

    public bool PrintListHasItems => PrintList?.Any() ?? false;

    [ObservableProperty]
    private ActionPrintViewModel _selectedPrintItem;

    [ObservableProperty]
    private ObservableCollection<ActionPrinterProfileViewModel> _printerProfiles = [];

    [RelayCommand]
    public void RefreshPrintList(ActionsTabName name)
    {
        switch (name)
        {
            case ActionsTabName.Print:
                FetchPrintActionsData();
                break;
        }
    }

    [RelayCommand]
    public void FetchPrintActionsData()
    {
        PrintList = new ObservableCollection<ActionPrintViewModel>
        {
            new() { Id = "1", JobName = "Print only drawings", IsSelected = false, PrintDrawingRange="0,5,7-8",IsPrintDrawing=true,Description="Prints only drawing files",
            DrawingExclusionList=$"Some Text;{Environment.NewLine}Some Text;{Environment.NewLine}Some Text",
             PrinterProfile=defaultProfile},
            new() { Id = "2", JobName = "Print ALL drawings scale to fit", IsSelected = true,IsPrintDrawing=true, Description="Prints drawing scaled to fit the paper",
             PrinterProfile=defaultProfile},
            new() { Id = "3", JobName = "Print 3D Models A3", IsSelected = false, IsPrintModel=true,Description="Prints models as 3D visuals",
             PrinterProfile=defaultProfile }
        };

        PrintList.CollectionChanged += (_, _) =>
        {
            OnPropertyChanged(nameof(PrintListHasItems));
        };

        if (PrintList.Any())
        {
            PrintList.First().IsSelected = true;

            foreach (var item in PrintList)
                item.SetSaveState();
        }

        PrinterProfiles = new ObservableCollection<ActionPrinterProfileViewModel>
        {
             defaultProfile,
            new (){ Copies=3, Name="Office Printer", Description=@"Office-Printer\HP LaserJet"},
            new (){ Copies=2, Name="Plotter", Description=@"Plotters\EPSON Stylus Pro"},
            new(){ Copies=1, Name="Home Printer", Description=@"Home-Printer\Canon Pixma" }
        };
    }


    protected override void OnDesignConstructor()
    {
        FetchPrintActionsData();
    }

    [RelayCommand]
    public void DeletePrintItem(string id)
    {
        bool flowControl = DeletePrintItemFromUI(id);
        if (!flowControl)
        {
            return;
        }
    }

    private bool DeletePrintItemFromUI(string id)
    {
        if (PrintList is null || PrintList.Count == 0)
            return false;

        var index = PrintList.ToList().FindIndex(x => x.Id == id);
        if (index == -1)
            return false;

        PrintList.RemoveAt(index);

        if (index > 0)
            index--;
        if (index >= 0 && PrintList.Count > index)
            PrintList[index].IsSelected = true;
        return true;
    }

    [RelayCommand]
    public void AddPrintItem()
    {
        var item = new ActionPrintViewModel()
        {
            Id = Guid.NewGuid().ToString(),
            IsSelected = true,
            IsNewItem = true,
            JobName = "New Job Print Item",
            PrinterProfile = defaultProfile
        };
        PrintList?.Add(item);
    }

    [RelayCommand]
    public void CancelPrintItem()
    {
        if (SelectedPrintItem is null) return;

        if (SelectedPrintItem.IsNewItem)
        {
            DeletePrintItemFromUI(SelectedPrintItem.Id);
        }
        else
        {
            //SelectedPrintItem.RevertChanges();
        }
    }
}
