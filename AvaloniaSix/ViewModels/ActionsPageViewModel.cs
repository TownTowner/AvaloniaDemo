using AvaloniaSix.Data;
using AvaloniaSix.Entities;
using AvaloniaSix.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;

namespace AvaloniaSix.ViewModels;

public partial class ActionsPageViewModel : PageViewModel
{
    private ActionPrintSettingsViewModel defaultSettings = new() { Id = "0", Copies = 1, Name = "(Default)PDF Printer", Description = @"Virtual Printers\Microsoft PDF" };

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PrintListHasItems))]
    private ObservableCollection<ActionPrintViewModel> _printList = [];

    public bool PrintListHasItems => PrintList?.Any() ?? false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedPrintItem))]
    private string? _selectedPrintItemId;

    public ActionPrintViewModel? SelectedPrintItem => PrintList?.FirstOrDefault(x => x.Id == SelectedPrintItemId);

    [ObservableProperty]
    private ObservableCollection<ActionPrintSettingsViewModel> _printSettings = [];

    private readonly MainViewModel _mainVM;
    private readonly DialogService _dialogService;
    private readonly PrinterService _printerService;
    private readonly DbService _dbService;

    public ActionsPageViewModel(MainViewModel mainVM,
        DialogService dialogService,
        PrinterService printerService,
        DbService dbService)
        : base(ApplicationPageName.Actions)
    {
        _mainVM = mainVM;
        _dialogService = dialogService;
        _printerService = printerService;
        _dbService = dbService;
    }

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

    private void FetchPrintSettings()
    {
        var settings = _dbService.GetPrintSettings();

        PrintSettings = settings.ToViewModels();
    }

    private void PrinterNamePropertyChanged(object? sender, PropertyChangedEventArgs args,
        ObservableCollection<PrinterDetailsViewModel> printers,
        ActionPrintSettingsProfileViewModel profileSetting)
    {
        if (args.PropertyName != nameof(ActionPrintSettingsProfileViewModel.PrinterName))
            return;

        var setting = sender as ActionPrintSettingsProfileViewModel ?? profileSetting;
        var printer = printers.FirstOrDefault(x => x.Name == setting.PrinterName);
        if (printer == null)
            return;

        setting.PaperSizeOptions = printer.PaperSizes;
        if (!setting.PaperSizeOptions.Contains(setting.PaperSize))
        {
            setting.PaperSize = setting.PaperSizeOptions.FirstOrDefault() ?? string.Empty;
        }
        setting.SourceTrayOptions = printer.SourceTrays;
        if (!setting.SourceTrayOptions.Contains(setting.SourceTray))
        {
            setting.SourceTray = setting.SourceTrayOptions.FirstOrDefault() ?? string.Empty;
        }
    }

    [RelayCommand]
    public void FetchPrintList()
    {
        FetchPrintSettings();

        var printList = _dbService.GetPrintList();

        var defaultPrinter = default(ActionPrintSettings);
        if (printList.Any(x => string.IsNullOrEmpty(x.ActionPrintSettingsId)))
            defaultPrinter = _dbService.GetPrintSettings().FirstOrDefault();

        PrintList = new(printList.Select(x =>
        {
            var vm = new ActionPrintViewModel()
            {
                Id = x.Id,
                JobName = x.JobName,
                Description = x.Description,
                PrintDrawingRange = x.PrintDrawingRange,
                DrawingExclusionList = x.DrawingExclusionList,
                DrawingExclusionIsWhiteList = x.DrawingExclusionIsWhiteList,
                IsPrintModel = x.IsPrintModel,
                IsPrintDrawing = x.IsPrintDrawing,
                PrintSettingsId = x.ActionPrintSettingsId ?? defaultPrinter?.Id
            };

            return vm;
        }));

        PrintList.CollectionChanged += (_, _) =>
        {
            OnPropertyChanged(nameof(PrintListHasItems));
        };

        if (PrintList.Any())
        {
            if (string.IsNullOrEmpty(SelectedPrintItemId))
                SelectedPrintItemId = PrintList.First().Id;

            foreach (var item in PrintList)
                item.SetSaveState();
        }
    }

    protected override void OnDesignConstructor()
    {
        FetchPrintList();
    }

    [RelayCommand]
    public void AddPrintItem()
    {
        var printers = _dbService.GetPrintSettings();
        var item = new ActionPrintViewModel()
        {
            Id = Guid.NewGuid().ToString("N"),
            IsNewItem = true,
            JobName = "New Print Item",
            PrintSettingsId = printers?.FirstOrDefault()?.Id
        };
        PrintList = PrintList ?? new();
        PrintList.Add(item);

        SelectedPrintItemId = item.Id;
    }

    [RelayCommand]
    public Task SavePrintItem()
    {
        if (SelectedPrintItem is null) return Task.CompletedTask;

        var entity = SelectedPrintItem.ToEntity();
        if (SelectedPrintItem.IsNewItem)
            entity = _dbService.AddPrintListItem(entity);
        else
            entity = _dbService.UpdatePrintListItem(entity);

        SelectedPrintItem.IsNewItem = false;
        SelectedPrintItem.SetSaveState();

        return Task.CompletedTask;
    }

    [RelayCommand]
    public async Task DeletePrintItemAsync(string id)
    {
        bool flowControl = await DeletePrintItemFromUiAsync(id, true);
        if (!flowControl) return;

        _dbService.DeletePrintListItem(id);
    }

    private async Task<bool> DeletePrintItemFromUiAsync(string id, bool warn = false)
    {
        if (PrintList is null || PrintList.Count == 0)
            return false;

        var index = PrintList.ToList().FindIndex(x => x.Id == id);
        if (index == -1)
            return false;

        if (warn)
        {
            var dialog = new ConfirmDialogViewModel
            {
                Title = $"Delete {PrintList[index].JobName}?",
                Message = "Are you sure you want to delete this print item?",
                //OnConfirmedAsync = async (vm) =>
                //{
                //    await Task.Delay(1000);
                //    vm.ProgressText = "This will take a while...";
                //    await Task.Delay(2000);
                //    vm.StatusText = "Unable to delete the item at this time.";

                //    return true;
                //}
            };
            await _dialogService.ShowDialogAsync(_mainVM, dialog);

            if (dialog.IsConfirmed == false)
                return false;
        }

        PrintList.RemoveAt(index);

        if (index > 0)
            index--;
        if (index >= 0 && PrintList.Count > index)
            SelectedPrintItemId = PrintList[index].Id;
        return true;
    }

    [RelayCommand]
    public async Task CancelPrintItemAsync()
    {
        if (SelectedPrintItem is null) return;

        if (SelectedPrintItem.IsNewItem)
        {
            await DeletePrintItemFromUiAsync(SelectedPrintItem.Id);
        }
        else
        {
            SelectedPrintItem.RestoreState();
        }
    }

    [RelayCommand]
    public async Task<bool> AddPrintSettingsAsync()
    {
        var profile = new ActionPrintSettingsViewModel
        {
            //Title = "Add Printer Profile",
            //Message = "Configure the printer profile settings below.",
            //    await Task.Delay(1000);
            //    vm.ProgressText = "This will take a while...";
            //    await Task.Delay(2000);
            //    vm.StatusText = "Unable to delete the item at this time.";

            //    return true;
            //}
        };

        InjectPrinterDetails(profile);

        await _dialogService.ShowDialogAsync(_mainVM, profile);

        if (profile.IsConfirmed == false)
            return false;

        PrintSettings.Add(profile);
        return true;
    }

    private void InjectPrinterDetails(ActionPrintSettingsViewModel profile)
    {
        var printers = _printerService.AvailablePrinters();
        foreach (var setting in profile.PrintSettingsProfiles)
        {
            setting.PropertyChanged +=
                (s, e) => PrinterNamePropertyChanged(s, e, printers, setting);
        }
    }

    [RelayCommand]
    public async Task EditPrintSettingsAsync(string id)
    {
        var profile = PrintSettings?.FirstOrDefault(x => x.Id == id);
        if (profile == null)
            return;

        // temporary copy to edit
        var copy = new ActionPrintSettingsViewModel();
        copy.RestoreState(profile.GetSaveState());

        InjectPrinterDetails(copy);

        await _dialogService.ShowDialogAsync(_mainVM, copy);

        if (profile.IsConfirmed == false)
            return;

        // apply changes when confirmed
        profile.RestoreState(copy.GetSaveState());
    }

    [RelayCommand]
    public async Task DeletePrintSettingsAsync(string id)
    {
        bool flowControl = await DeletePrintSettingsFromUiAsync(id, true);
        if (!flowControl) return;

        _dbService.DeletePrintSettings(id);
    }

    private async Task<bool> DeletePrintSettingsFromUiAsync(string id, bool warn = false)
    {
        if (PrintSettings is null || PrintSettings.Count == 0)
            return false;

        var index = PrintSettings.ToList().FindIndex(x => x.Id == id);
        if (index == -1)
            return false;

        if (warn)
        {
            var dialog = new ConfirmDialogViewModel
            {
                DialogWidth = 500,
                Title = "Delete print profile!",
                Message = $"Are you sure you want to delete '{PrintSettings[index].Name}' ?",
            };
            await _dialogService.ShowDialogAsync(_mainVM, dialog);

            if (dialog.IsConfirmed == false)
                return false;
        }

        PrintSettings.RemoveAt(index);

        if (index > 0)
            index--;
        if (index >= 0 && PrintSettings.Count > index && SelectedPrintItem != null)
            SelectedPrintItem.PrintSettingsId = PrintSettings[index].Id;

        return true;
    }
}
