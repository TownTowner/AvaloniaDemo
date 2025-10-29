using AvaloniaSix.Data;
using AvaloniaSix.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace AvaloniaSix.ViewModels;

public partial class ActionsPageViewModel : PageViewModel
{
    private ActionPrintSettingsViewModel defaultProfile = new() { Id = "0", Copies = 1, Name = "(Default)PDF Printer", Description = @"Virtual Printers\Microsoft PDF" };

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PrintListHasItems))]
    private ObservableCollection<ActionTabPrintViewModel> _printList = [];

    public bool PrintListHasItems => PrintList?.Any() ?? false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedPrintItem))]
    private string? _selectedPrintItemId;

    public ActionTabPrintViewModel? SelectedPrintItem => PrintList?.FirstOrDefault(x => x.Id == SelectedPrintItemId);

    [ObservableProperty]
    private ObservableCollection<ActionPrintSettingsViewModel> _printerProfiles = [];

    private readonly MainViewModel _mainVM;
    private readonly DialogService _dialogService;
    private readonly PrinterService printerService;

    public ActionsPageViewModel(MainViewModel mainVM,
        DialogService dialogService,
        PrinterService printerService)
        : base(ApplicationPageName.Actions)
    {
        _mainVM = mainVM;
        _dialogService = dialogService;
        this.printerService = printerService;
    }

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

    private void FetchPrintProfiles()
    {
        var printers = printerService.AvailablePrinters();
        var printerOptions = new ObservableCollection<string>(printers.Select(x => x.Name));

        var profileSetting = new ActionPrintSettingsProfileViewModel()
        {
            Id = "0",
            Width = 210,
            Height = 297,
            Orientation = "Portrait",
            PaperSize = "A4",
            PrinterNameOptions = printerOptions
        };
        profileSetting.PropertyChanged += (s, a) => PrinterNamePropertyChanged(s, a, printers, profileSetting);

        var profileSettingList = new ObservableCollection<ActionPrintSettingsProfileViewModel> { profileSetting, profileSetting, profileSetting };

        defaultProfile.PrintSettingsProfiles = profileSettingList;
        PrinterProfiles = new ObservableCollection<ActionPrintSettingsViewModel>
        {
            defaultProfile,
            new (){Id="1", Copies=3, Name="Office Printer", Description=@"Office-Printer\HP LaserJet",PrintSettingsProfiles=profileSettingList},
            new (){Id="2", Copies=2, Name="Plotter", Description=@"Plotters\EPSON Stylus Pro", PrintSettingsProfiles=profileSettingList},
            new(){Id="3", Copies=1, Name="Home Printer", Description=@"Home-Printer\Canon Pixma",PrintSettingsProfiles=profileSettingList }
        };
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
    public void FetchPrintActionsData()
    {
        FetchPrintProfiles();

        PrintList = new ObservableCollection<ActionTabPrintViewModel>
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
    public async Task DeletePrintItemAsync(string id)
    {
        bool flowControl = await DeletePrintItemFromUiAsync(id, true);
        if (!flowControl)
        {
            return;
        }
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
    public void AddPrintItem()
    {
        var item = new ActionTabPrintViewModel()
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
    public async Task<bool> AddPrinterSettingsAsync()
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

        PrinterProfiles.Add(profile);
        return true;
    }

    private void InjectPrinterDetails(ActionPrintSettingsViewModel profile)
    {
        var printers = printerService.AvailablePrinters();
        foreach (var setting in profile.PrintSettingsProfiles)
        {
            setting.PropertyChanged +=
                (s, e) => PrinterNamePropertyChanged(s, e, printers, setting);
        }
    }

    [RelayCommand]
    public async Task EditPrinterSettingsAsync(string id)
    {
        var profile = PrinterProfiles?.FirstOrDefault(x => x.Id == id);
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
    public async Task DeletePrinterSettings(string id)
    {
        bool flowControl = await DeletePrinterProfileFromUiAsync(id, true);
        if (!flowControl)
        {
            return;
        }
    }

    private async Task<bool> DeletePrinterProfileFromUiAsync(string id, bool warn = false)
    {
        if (PrinterProfiles is null || PrinterProfiles.Count == 0)
            return false;

        var index = PrinterProfiles.ToList().FindIndex(x => x.Id == id);
        if (index == -1)
            return false;

        if (warn)
        {
            var dialog = new ConfirmDialogViewModel
            {
                DialogWidth = 500,
                Title = "Delete print profile!",
                Message = $"Are you sure you want to delete '{PrinterProfiles[index].Name}' ?",
            };
            await _dialogService.ShowDialogAsync(_mainVM, dialog);

            if (dialog.IsConfirmed == false)
                return false;
        }

        PrinterProfiles.RemoveAt(index);

        if (index > 0)
            index--;
        if (index >= 0 && PrinterProfiles.Count > index && SelectedPrintItem != null)
            SelectedPrintItem.PrinterProfileId = PrinterProfiles[index].Id;

        return true;
    }
}
