using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvaloniaSix.Entities;

public class ActionPrint : ActionEntityBase
{
    [MaxLength(500)]
    public string PrintDrawingRange { get; set; } = "";

    [MaxLength(1000)]
    public string DrawingExclusionList { get; set; } = "";

    public bool DrawingExclusionIsWhiteList { get; set; }

    public bool IsPrintModel { get; set; }

    public bool IsPrintDrawing { get; set; }

    [MaxLength(100)]
    public string? ActionPrintSettingsId { get; set; } = "";

    [ForeignKey(nameof(ActionPrintSettingsId))]
    public ActionPrintSettings? ActionPrintSettings { get; set; }
}