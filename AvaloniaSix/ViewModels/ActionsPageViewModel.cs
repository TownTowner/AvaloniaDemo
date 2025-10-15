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
            new() { Id = "1", JobName = "Print Job 1", IsSelected = false },
            new() { Id = "2", JobName = "Print Job 2", IsSelected = true },
            new() { Id = "3", JobName = "Print Job 3", IsSelected = false }
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

}
