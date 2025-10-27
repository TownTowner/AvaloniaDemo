using AvaloniaSix.Data;
using AvaloniaSix.Entities;
using AvaloniaSix.Factories;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace AvaloniaSix.ViewModels;

public partial class SettingsPageViewModel : PageViewModel
{
    private DbFactory _dbFactory;

    [ObservableProperty] private bool _skipNoActionFiles;

    [ObservableProperty] private bool _allowDuplicateEntries;

    [ObservableProperty] private ObservableCollection<string> _drawingTemplateSearchPaths = [];

    // TODO: Fetch from PDME
    [ObservableProperty] private ObservableCollection<string> _pdmeVaultNames = ["Vault 1", "Vault 2", "Vault 3"];

    [ObservableProperty] private string _pdmeVaultName = "";
    [ObservableProperty] private string _pdmeUsername = "";
    [ObservableProperty] private string _pdmePassword = "";

    [ObservableProperty] private string _solidWorksHost = "";

    // TODO: Fetch from network pings
    [ObservableProperty] private ObservableCollection<string> _solidWorksHosts = ["localhost", "127.0.0.1", "192.168.0.10"];

    [ObservableProperty]
    private ObservableCollection<string> _locationPaths = [];

    public SettingsPageViewModel()
        : this(new DbFactory(() => new(new())))
    {
        // Parameterless constructor for design-time tools
    }

    public SettingsPageViewModel(DbFactory dbFactory) : base(ApplicationPageName.Settings)
    {
        _dbFactory = dbFactory;

        LoadSettings();
    }

    private string[] NoticingProperties { get; set; } = [nameof(SkipNoActionFiles), nameof(AllowDuplicateEntries), nameof(SolidWorksHost)];

    public override void OnViewLoaded()
    {
        base.OnViewLoaded();

        PropertyChanged += (s, e) =>
        {
            if (NoticingProperties.Contains(e.PropertyName))
                SaveSettings();
        };
    }

    [RelayCommand]
    private void PdmLogin()
    {
        SaveSettings();
    }

    private void LoadSettings()
    {
        using var dbService = _dbFactory.GetDbService();
        var setting = dbService.GetSetting();
        LocationPaths = new(setting?.LocationPaths ?? []);
        SolidWorksHost = setting?.SolidWorksHost ?? "";
        PdmeVaultName = setting?.PdmeVaultName ?? "";
        PdmeUsername = setting?.PdmeUsername ?? "";
        PdmePassword = setting?.PdmePassword ?? "";
        SkipNoActionFiles = setting?.SkipNoActionFiles ?? false;
        AllowDuplicateEntries = setting?.AllowDuplicateEntries ?? false;
        DrawingTemplateSearchPaths = new(setting?.DrawingTemplatePaths ?? []);
    }

    private void SaveSettings()
    {
        using var dbService = _dbFactory.GetDbService();
        var setting = ToEntity();
        dbService.SaveSetting(setting);
    }

    private Setting ToEntity() => new()
    {
        LocationPaths = [.. LocationPaths],
        SolidWorksHost = SolidWorksHost,
        PdmeVaultName = PdmeVaultName,
        PdmeUsername = PdmeUsername,
        PdmePassword = PdmePassword,
        SkipNoActionFiles = SkipNoActionFiles,
        AllowDuplicateEntries = AllowDuplicateEntries,
        DrawingTemplatePaths = [.. DrawingTemplateSearchPaths]
    };
}
