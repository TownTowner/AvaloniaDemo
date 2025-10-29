using System.Collections.Generic;

namespace AvaloniaSix.Entities;

public class ActionPrintSettings : ActionEntityBase
{
    public bool CanEdit { get; init; }

    public bool CanDelete { get; init; }

    public List<ActionPrintSettingsProfile> ActionPrintSettingsProfiles { get; init; }

    public List<ActionTabPrint> ActionTabPrints { get; init; }

    public int Copies { get; init; }
}