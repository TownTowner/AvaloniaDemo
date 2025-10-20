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
    private ActionPrinterProfileViewModel defaultProfile = new() { Id = "0", Copies = 1, Name = "(Default)PDF Printer", Description = @"Virtual Printers\Microsoft PDF" };

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PrintListHasItems))]
    private ObservableCollection<ActionPrintViewModel> _printList = [];

    public bool PrintListHasItems => PrintList?.Any() ?? false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedPrintItem))]
    private string _selectedPrintItemId;

    public ActionPrintViewModel SelectedPrintItem => PrintList?.FirstOrDefault(x => x.Id == SelectedPrintItemId);

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
        PrinterProfiles = new ObservableCollection<ActionPrinterProfileViewModel>
        {
             defaultProfile,
            new (){Id="1", Copies=3, Name="Office Printer", Description=@"Office-Printer\HP LaserJet"},
            new (){Id="2", Copies=2, Name="Plotter", Description=@"Plotters\EPSON Stylus Pro"},
            new(){Id="3", Copies=1, Name="Home Printer", Description=@"Home-Printer\Canon Pixma" }
        };

        PrintList = new ObservableCollection<ActionPrintViewModel>
        {
            new() { Id = "1", JobName = "Print only drawings",  PrintDrawingRange="0,5,7-8",IsPrintDrawing=true,Description="Prints only drawing files",
            DrawingExclusionList=$"Some Text;{Environment.NewLine}Some Text;{Environment.NewLine}Some Text",
             PrinterProfileId=defaultProfile.Id},
            new() { Id = "2", JobName = "Print ALL drawings scale to fit",IsPrintDrawing=true, Description="Prints drawing scaled to fit the paper",
             PrinterProfileId=defaultProfile.Id},
            new() { Id = "3", JobName = "Print 3D Models A3", IsPrintModel=true,Description="Prints models as 3D visuals",
             PrinterProfileId=defaultProfile.Id }
        };

        PrintList.CollectionChanged += (_, _) =>
        {
            OnPropertyChanged(nameof(PrintListHasItems));
        };

        if (PrintList.Any())
        {
            SelectedPrintItemId = PrintList.First().Id;

            foreach (var item in PrintList)
                item.SetSaveState();
        }
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
            SelectedPrintItemId = PrintList[index].Id;
        return true;
    }

    [RelayCommand]
    public void AddPrintItem()
    {
        var item = new ActionPrintViewModel()
        {
            Id = Guid.NewGuid().ToString(),
            IsNewItem = true,
            JobName = "New Job Print Item",
            PrinterProfileId = defaultProfile.Id
        };
        SelectedPrintItemId = item.Id;
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
            SelectedPrintItem.RestoreState();
        }
    }
}
