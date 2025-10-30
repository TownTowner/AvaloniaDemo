using AvaloniaSix.Entities;
using AvaloniaSix.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaSix.Services;

public class DbService : IDisposable
{
    private AvaSixDbContext _dbContext;

    public DbService(AvaSixDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void ApplyMigrations()
    {
        _dbContext.Database.EnsureCreated();

        //_dbContext.Database.Migrate();
    }

    #region Settings
    public Setting GetSetting()
    {
        var setting = _dbContext.Settings.FirstOrDefault();
        if (setting != null) return setting;

        setting = new()
        {
            LocationPaths = ["Initial Path1", "Initial Path2", "Initial Path3"]
        };
        SaveSetting(setting);

        return setting;
    }

    public void SaveSetting(Setting setting)
    {
        _dbContext.Settings.RemoveRange(_dbContext.Settings);

        _dbContext.Settings.Add(setting);
        _dbContext.SaveChanges();
    }
    #endregion

    #region Print Tabs
    public List<ActionPrint> GetPrintList()
    {
        var printList = _dbContext.ActionPrints.AsNoTracking().ToList();
        if (printList.Any()) return printList;

        var setting = _dbContext.ActionPrintSettingses.FirstOrDefault();
        printList.Add(new()
        {
            JobName = "Sample Print Job",
            Description = "This is a sample print job.",
            PrintDrawingRange = "1-10",
            DrawingExclusionList = "2,4,6",
            DrawingExclusionIsWhiteList = false,
            IsPrintModel = false,
            IsPrintDrawing = true,
            ActionPrintSettingsId = setting?.Id
        });

        return printList;
    }

    public ActionPrint AddPrintListItem(ActionPrint entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        _dbContext.ActionPrints.Add(entity);

        _dbContext.SaveChanges();

        return entity;
    }

    public ActionPrint UpdatePrintListItem(ActionPrint entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        ArgumentException.ThrowIfNullOrEmpty(entity.Id, nameof(entity.Id));

        var check = _dbContext.ActionPrints.Find(entity.Id);
        if (check != null)
            _dbContext.ActionPrints.Remove(check);

        _dbContext.ActionPrints.Add(entity);
        _dbContext.SaveChanges();

        return entity;
    }

    public void DeletePrintListItem(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var check = _dbContext.ActionPrints.Find(id);
        if (check == null) return;

        _dbContext.ActionPrints.Remove(check);
        _dbContext.SaveChanges();
    }

    #endregion

    #region Print Settings

    public List<ActionPrintSettingsProfile> GetPrintSettingsProfiles()
    {
        return [
            new() { Type = "A0Size" },
            new() { Type = "A1Size" },
            new() { Type = "A2Size" },
            new() { Type = "A3Size" },
            new() { Type = "A4Size" },
            new() { Type = "A4VerticalSize" },
            new() { Type = "ASize" },
            new() { Type = "AVerticalSize" },
            new() { Type = "BSize" },
            new() { Type = "CSize" },
            new() { Type = "DSize" },
            new() { Type = "ESize" },
            new() { Type = "UserSize1" },
            new() { Type = "UserSize2" },
            new() { Type = "UserSize3" },
            new() { Type = "UserSize4" },
            new() { Type = "UserSize5" },
            new() { Type = "UserSize6" },
            new() { Type = "UserSize7" },
            new() { Type = "UserSize8" },
            new() { Type = "UserSize9" },
            new() { Type = "UserSize10" },
            new() { Type = "UserSize11" },
            new() { Type = "UserSize12" }
        ];
        //profileSetting.PropertyChanged += (s, a) => PrinterNamePropertyChanged(s, a, printers, profileSetting);

        //var profileSettingList = new ObservableCollection<ActionPrintSettingsProfileViewModel> { profileSetting, profileSetting, profileSetting };
    }

    public List<ActionPrintSettings> GetPrintSettings()
    {
        //defaultSettings.PrintSettingsProfiles = profileSettingList;
        //PrintSettings = new ObservableCollection<ActionPrintSettingsViewModel>
        //{
        //    defaultSettings,
        //    new (){Id="1", Copies=3, Name="Office Printer", Description=@"Office-Printer\HP LaserJet",PrintSettingsProfiles=profileSettingList},
        //    new (){Id="2", Copies=2, Name="Plotter", Description=@"Plotters\EPSON Stylus Pro", PrintSettingsProfiles=profileSettingList},
        //    new(){Id="3", Copies=1, Name="Home Printer", Description=@"Home-Printer\Canon Pixma",PrintSettingsProfiles=profileSettingList }
        //};
        var settings = _dbContext.ActionPrintSettingses
            .Include(f => f.ActionPrintSettingsProfiles).ToList();

        if (settings.Any()) return settings;

        // Add default settings
        _dbContext.ActionPrintSettingses.Add(new()
        {
            JobName = "(Default)",
            Description = "Use all default settings",
            Copies = 1,
            ActionPrintSettingsProfiles = GetPrintSettingsProfiles()
        });

        _dbContext.SaveChanges();

        settings = _dbContext.ActionPrintSettingses
            .Include(f => f.ActionPrintSettingsProfiles).ToList();

        return settings;
    }

    public ActionPrintSettings AddPrintSettings(ActionPrintSettings entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        _dbContext.ActionPrintSettingses.Add(entity);

        _dbContext.SaveChanges();

        return entity;
    }

    public ActionPrintSettings UpdatePrintSettings(ActionPrintSettings entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        ArgumentException.ThrowIfNullOrEmpty(entity.Id, nameof(entity.Id));

        var check = _dbContext.ActionPrintSettingses.Find(entity.Id);
        if (check != null)
            _dbContext.ActionPrintSettingses.Remove(check);

        _dbContext.ActionPrintSettingses.Add(entity);
        _dbContext.SaveChanges();

        return entity;
    }

    public void DeletePrintSettings(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        var check = _dbContext.ActionPrintSettingses.Find(id);
        if (check == null) return;

        _dbContext.ActionPrintSettingses.Remove(check);
        _dbContext.SaveChanges();
    }

    #endregion


    public void Dispose()
    {
        _dbContext?.Dispose();
    }
}
