using AvaloniaSix.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AvaloniaSix.ViewModels;

public static class ActionPrintSettingsViewModelExtensions
{
    public static ActionPrintSettings ToEntity(this ActionPrintSettingsViewModel viewModel)
    {
        return new ActionPrintSettings()
        {
            Id = viewModel.Id,
            CanDelete = viewModel.CanDelete,
            CanEdit = viewModel.CanEdit,
            Copies = viewModel.Copies,
            Description = viewModel.Description,
            JobName = viewModel.Name,
            ActionPrintSettingsProfiles = viewModel.PrintSettingsProfiles.ToEntities()
        };
    }

    public static List<ActionPrintSettings> ToEntities(
        this ObservableCollection<ActionPrintSettingsViewModel> viewModels) =>
        viewModels.Select(ToEntity).ToList();

    public static ActionPrintSettingsViewModel ToViewModel(this ActionPrintSettings entity)
    {
        return new()
        {
            Name = entity.JobName,
            Description = entity.Description,
            Id = entity.Id,
            Copies = entity.Copies,
            CanEdit = entity.CanEdit,
            CanDelete = entity.CanDelete,
            PrintSettingsProfiles = new(entity.ActionPrintSettingsProfiles
                .OrderBy(profile => profile.Type)
                .Select(profile => new ActionPrintSettingsProfileViewModel
                {
                    Id = profile.Id,
                    DrawingColor = profile.DrawingColor,
                    Height = profile.Height,
                    Orientation = profile.Orientation,
                    PaperSize = profile.PaperSize,
                    PrinterName = profile.PrinterName,
                    ScaleToFit = profile.ScaleToFit,
                    SourceTray = profile.SourceTray,
                    Type = profile.Type,
                    Width = profile.Width,
                }))
        };
    }

    public static ObservableCollection<ActionPrintSettingsViewModel> ToViewModels(
        this List<ActionPrintSettings> entities)
    {
        return new(entities
            .OrderBy(f => f.JobName)
            .Select(ToViewModel));
    }
}

public static class ActionPrintSettingsProfileViewModelExtensions
{
    public static ActionPrintSettingsProfile ToEntity(this ActionPrintSettingsProfileViewModel viewModel)
    {
        return new()
        {
            Id = viewModel.Id,
            Type = viewModel.Type,
            PrinterName = viewModel.PrinterName,
            DrawingColor = viewModel.DrawingColor,
            Height = viewModel.Height,
            Width = viewModel.Width,
            Orientation = viewModel.Orientation,
            SourceTray = viewModel.SourceTray,
            PaperSize = viewModel.PaperSize,
            ScaleToFit = viewModel.ScaleToFit
        };
    }

    public static List<ActionPrintSettingsProfile> ToEntities(
        this ObservableCollection<ActionPrintSettingsProfileViewModel> viewModels) =>
        viewModels.Select(ToEntity).ToList();

    public static ActionPrintSettingsProfileViewModel ToViewModel(this ActionPrintSettingsProfile entity)
    {
        return new()
        {
            Id = entity.Id,
            Type = entity.Type,
            PrinterName = entity.PrinterName,
            DrawingColor = entity.DrawingColor,
            Height = entity.Height,
            Width = entity.Width,
            Orientation = entity.Orientation,
            SourceTray = entity.SourceTray,
            PaperSize = entity.PaperSize,
            ScaleToFit = entity.ScaleToFit
        };
    }

    public static ObservableCollection<ActionPrintSettingsProfileViewModel> ToViewModels(
        this List<ActionPrintSettingsProfile> entities) =>
        new(entities.Select(ToViewModel).ToList());

}