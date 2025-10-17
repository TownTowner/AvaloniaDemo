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
    [ObservableProperty]
    private ObservableCollection<ActionPrintViewModel> _printList;

    [ObservableProperty]
    private ActionPrintViewModel _selectedPrintItem;

    [RelayCommand]
    public void RefreshPrintList(ActionsTabName name)
    {
        switch (name)
        {
            case ActionsTabName.Print:
                FetchPrintList();
                break;
        }
    }

    [RelayCommand]
    public void FetchPrintList()
    {
        PrintList = new ObservableCollection<ActionPrintViewModel>
        {
            new() { Id = "1", JobName = "Print only drawings", IsSelected = false, PrintDrawingRange="0,5,7-8",IsPrintDrawing=true,Description="Prints only drawing files",
            DrawingExclusionList=$"Some Text;{Environment.NewLine}Some Text;{Environment.NewLine}Some Text"},
            new() { Id = "2", JobName = "Print ALL drawings scale to fit", IsSelected = true,IsPrintDrawing=true, Description="Prints drawing scaled to fit the paper"},
            new() { Id = "3", JobName = "Print 3D Models A3", IsSelected = false, IsPrintModel=true,Description="Prints models as 3D visuals" }
        };
    }

    protected override void OnDesignConstructor()
    {
        FetchPrintList();
    }

    [RelayCommand]
    public void DeletePrintItem(string id)
    {
        PrintList?.Remove(PrintList.FirstOrDefault(x => x.Id == id));
    }

    [RelayCommand]
    public void AddPrintItem()
    {
        var item = new ActionPrintViewModel()
        {
            IsSelected = true,
            IsNewItem = true,
            JobName = "New Job Print Item"
        };
        PrintList?.Add(item);
    }

}
