using AvaloniaSix.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AvaloniaSix.ViewModels;

public static class ActionViewModelExtensions
{
    public static ActionViewModelBase ToViewModel(this ActionEntityBase entityBase) =>
    new()
    {
        Id = entityBase.Id,
        JobName = entityBase.JobName,
        Description = entityBase.Description,
        SortOrder = entityBase.SortOrder
    };

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
                .Select(ToViewModel))
        };
    }

    public static ObservableCollection<ActionPrintSettingsViewModel> ToViewModels(
        this List<ActionPrintSettings> entities)
    {
        return new(entities
            .OrderBy(f => f.JobName)
            .Select(ToViewModel));
    }

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
        new(entities.Select(ToViewModel));

    public static ActionPrint ToEntity(this ActionPrintViewModel viewModel) => new()
    {
        Id = viewModel.Id,
        Description = viewModel.Description,
        DrawingExclusionIsWhiteList = viewModel.DrawingExclusionIsWhiteList,
        DrawingExclusionList = viewModel.DrawingExclusionList,
        JobName = viewModel.JobName,
        PrintDrawingRange = viewModel.PrintDrawingRange,
        IsPrintDrawing = viewModel.IsPrintDrawing,
        IsPrintModel = viewModel.IsPrintModel,
        ActionPrintSettingsId = viewModel.PrintSettingsId,
    };

    public static ActionPrintViewModel ToViewModel(this ActionPrint entity) =>
        new()
        {
            Id = entity.Id,
            JobName = entity.JobName,
            Description = entity.Description,
            DrawingExclusionIsWhiteList = entity.DrawingExclusionIsWhiteList,
            DrawingExclusionList = entity.DrawingExclusionList,
            PrintDrawingRange = entity.PrintDrawingRange,
            IsPrintDrawing = entity.IsPrintDrawing,
            IsPrintModel = entity.IsPrintModel,
            PrintSettingsId = entity.ActionPrintSettingsId,
        };
}