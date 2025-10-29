using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvaloniaSix.Entities;

public class ActionPrintSettingsProfile : EntityBase
{
    [MaxLength(100)]
    public string ActionPrintSettingsId { get; init; } = "";

    [ForeignKey(nameof(ActionPrintSettingsId))]
    public ActionPrintSettings? ActionPrintSettings { get; init; }

    [MaxLength(100)]
    public string Type { get; init; } = "";

    [MaxLength(500)]
    public string PrinterName { get; init; } = "(Default)";

    [MaxLength(100)]
    public string PaperSize { get; init; } = "(Default)";

    public double Width { get; init; } = -1;

    public double Height { get; init; } = -1;

    [MaxLength(100)]
    public string Orientation { get; init; } = "(Default)";

    [MaxLength(100)]
    public string SourceTray { get; init; } = "(Default)";

    [MaxLength(100)]
    public string DrawingColor { get; init; } = "(Default)";

    public bool ScaleToFit { get; init; }
}
